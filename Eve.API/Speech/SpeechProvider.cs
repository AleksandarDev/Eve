using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Eve.Core.Kinect;
using Microsoft.Kinect;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;

namespace Eve.API.Speech {
	public static class SpeechProvider {
		//
		// Speech
		//
		private static SpeechRecognitionEngine speechRecognizer;
		private static SpeechSynthesizer speechSynthesizer;
		private static Stream audioSource;

	
		public async static Task Start() {
			await SpeechProvider.InitializeSpeechRecognizer();
			await SpeechProvider.InitializeSpeechSynthesizer();

			SpeechProvider.IsRunning = true;

			System.Diagnostics.Debug.WriteLine("Speech Provider started", typeof(SpeechProvider).Name);
		}

		public static async Task Stop() {
			await Task.Run(() => {
				if (SpeechProvider.speechRecognizer != null) {
					SpeechProvider.speechRecognizer.SpeechRecognized -= SpeechRecognized;
					SpeechProvider.speechRecognizer.SpeechRecognitionRejected -= SpeechRejected;
					SpeechProvider.speechRecognizer.SpeechHypothesized -= SpeechHypothesized;
					SpeechProvider.speechRecognizer.SpeechDetected -= SpeechDetected;
					SpeechProvider.speechRecognizer.RecognizeAsyncCancel();
					SpeechProvider.speechRecognizer = null;
				}

				SpeechProvider.speechSynthesizer = null;

				if (SpeechProvider.audioSource != null)
					SpeechProvider.audioSource.Dispose();
				SpeechProvider.audioSource = null;

				SpeechProvider.IsRunning = false;

				System.Diagnostics.Debug.WriteLine("Speech Provider stopped!", typeof(SpeechProvider).Name);
			});
		}

		public async static Task Speak(SpeechPrompt speechPrompt) {
			if (!SpeechProvider.IsRunning)
				await SpeechProvider.Start();

			if (SpeechProvider.OnSynthesisRequested != null)
				SpeechProvider.OnSynthesisRequested(new SpeechProviderSynthesizerEventArgs() {
					Synthesizer = SpeechProvider.speechSynthesizer,
					Prompt = speechPrompt
				});

			if (SpeechProvider.IsRunning && speechPrompt != null) {
				if (SpeechProvider.OnSynthesisStarted != null)
					SpeechProvider.OnSynthesisStarted(new SpeechProviderSynthesizerEventArgs() {
						Synthesizer = SpeechProvider.speechSynthesizer,
						Prompt = speechPrompt
					});

				// TODO Check if service needs to be stoped before synthesis
				//if (SpeechProvider.speechRecognizer != null) {
				//	System.Diagnostics.Debug.WriteLine("Stopping speech recognition...", typeof(SpeechProvider).Name);
				//	SpeechProvider.speechRecognizer.RecognizeAsyncCancel();
				//	System.Diagnostics.Debug.WriteLine("Speech recognition stopped", typeof(SpeechProvider).Name);
				//}

				System.Diagnostics.Debug.WriteLine("Speech request executing...", typeof (SpeechProvider).Name);
				await Task.Run(() => SpeechProvider.speechSynthesizer.Speak(speechPrompt.Prompt));
				System.Diagnostics.Debug.WriteLine("Speech request executed", typeof (SpeechProvider).Name);

				//if (SpeechProvider.speechRecognizer != null) {
				//	System.Diagnostics.Debug.WriteLine("Starting speech recognition...", typeof(SpeechProvider).Name);
				//	SpeechProvider.speechRecognizer.RecognizeAsync(RecognizeMode.Multiple);
				//	System.Diagnostics.Debug.WriteLine("Speech recognition started", typeof(SpeechProvider).Name);
				//}

				if (SpeechProvider.OnSynthesisEnded != null)
					SpeechProvider.OnSynthesisEnded(new SpeechProviderSynthesizerEventArgs() {
						Synthesizer = SpeechProvider.speechSynthesizer,
						Prompt = speechPrompt
					});
			}
		}

		private async static Task InitializeSpeechSynthesizer() {
			await Task.Run(() => {
				SpeechProvider.speechSynthesizer = new SpeechSynthesizer();
				SpeechProvider.speechSynthesizer.SetOutputToDefaultAudioDevice();
				SpeechProvider.speechSynthesizer.SelectVoice(GetSpeechSynthesizerVoice());
				SpeechProvider.speechSynthesizer.Rate = 0;

				System.Diagnostics.Debug.WriteLine("Speech Synthesizer created!", typeof (SpeechProvider).Name);
			});
		}

		private async static Task InitializeSpeechRecognizer() {
			await Task.Run(() => {
				// Get recognizer info
				var recognizerInfo = GetRecognizerInfo();
				if (recognizerInfo == null) {
					System.Diagnostics.Debug.WriteLine("No recognizer data found!", typeof(SpeechProvider).Name);
					// TODO Throw exception
					return;
				}

				// Setup recognizer
				SpeechProvider.speechRecognizer = new SpeechRecognitionEngine(recognizerInfo.Id);
				SpeechProvider.speechRecognizer.SpeechRecognized += SpeechRecognized;
				SpeechProvider.speechRecognizer.SpeechRecognitionRejected += SpeechRejected;
				SpeechProvider.speechRecognizer.SpeechHypothesized += SpeechHypothesized;
				SpeechProvider.speechRecognizer.SpeechDetected += SpeechDetected;

				// Create a grammar from grammar definition XML file.
				using (var stream = File.OpenRead("Speech/Grammars/EveGrammar.xml")) {
					var g = new Grammar(stream);
					speechRecognizer.LoadGrammar(g);
				}

				// Get audio source
				if (KinectService.HasSensorAvailable) {
					var kinectAudioSource = KinectService.GetAudioSource(0);
					if (kinectAudioSource == null) {
						System.Diagnostics.Debug.WriteLine("Can't get audio source for SpeechProvider!", typeof(SpeechProvider).Name);
						// TODO throw exception
						return;
					}

					// Try to get Kinect as audio source
					kinectAudioSource.EchoCancellationMode = EchoCancellationMode.CancellationAndSuppression;
					kinectAudioSource.NoiseSuppression = true;
					kinectAudioSource.AutomaticGainControlEnabled = true;

					SpeechProvider.audioSource = kinectAudioSource.Start();
				}
				else {
					SpeechProvider.speechRecognizer.SetInputToDefaultAudioDevice();
				}
				// TODO Implement using KinectService
				//SpeechProvider.speechRecognizer.SetInputToDefaultAudioDevice();

				if (SpeechProvider.audioSource != null && SpeechProvider.audioSource.CanRead)
					SpeechProvider.speechRecognizer.SetInputToAudioStream(
						SpeechProvider.audioSource, new SpeechAudioFormatInfo(EncodingFormat.Pcm,
						                                                       16000, 16, 1, 32000, 2, null));


				// Initiate recognition
				SpeechProvider.speechRecognizer.RecognizeAsync(RecognizeMode.Multiple);

				System.Diagnostics.Debug.WriteLine("Speech Recognizer created!", typeof(SpeechProvider).Name);
			});
		}

		private static void SpeechDetected(object sender, SpeechDetectedEventArgs e) {
			System.Diagnostics.Debug.WriteLine("Speech detected...", typeof(SpeechProvider).Name);

			if (SpeechProvider.OnRecognitionDetected != null)
				SpeechProvider.OnRecognitionDetected(new SpeechProviderRecognozerEventArgs() {
					Recognizer = SpeechProvider.speechRecognizer
				});
		}

		private static void SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e) {
			System.Diagnostics.Debug.WriteLine(String.Format("Hypothesized \"{0}\"", e.Result.Text), typeof(SpeechProvider).Name);

			if (SpeechProvider.OnRecognitionHypothesized != null)
				SpeechProvider.OnRecognitionHypothesized(new SpeechProviderRecognozerEventArgs() {
					Recognizer = SpeechProvider.speechRecognizer,
					Result = e.Result
				});
		}

		private static void SpeechRejected(object sender, SpeechRecognitionRejectedEventArgs e) {
			System.Diagnostics.Debug.WriteLine(String.Format("Rejected \"{0}\"", e.Result.Text), typeof(SpeechProvider).Name);

			if (SpeechProvider.OnRecognitionRejected != null)
					SpeechProvider.OnRecognitionRejected(new SpeechProviderRecognozerEventArgs() {
						Recognizer = SpeechProvider.speechRecognizer,
						Result = e.Result
					});
		}

		private static void SpeechRecognized(object sender, SpeechRecognizedEventArgs e) {
			if (e.Result.Confidence >= SpeechProvider.SpeechRecognitionConfidence) {
				System.Diagnostics.Debug.WriteLine(
					String.Format("Accepted \"{0}\" for {1}%", e.Result.Text, e.Result.Confidence*100),
					typeof (SpeechProvider).Name);

				if (SpeechProvider.OnRecognitionAccepted != null)
					SpeechProvider.OnRecognitionAccepted(new SpeechProviderRecognozerEventArgs() {
						Recognizer = SpeechProvider.speechRecognizer,
						Result = e.Result
					});
			}
		}

		/// <summary>
		/// Gets the metadata for the speech recognizer (acoustic model) most suitable to
		/// process audio from Kinect device.
		/// </summary>
		/// <returns>
		/// RecognizerInfo if found, <code>null</code> otherwise.
		/// </returns>
		private static RecognizerInfo GetRecognizerInfo() {
			var recognizersAvailable = SpeechRecognitionEngine.InstalledRecognizers();

			// Check if any recognizer available
			if (recognizersAvailable.Count == 0)
				return null;


			// Match to Kinect for en-US
			var kinectMatches = recognizersAvailable.Where(r => r.AdditionalInfo.Values.Contains("Kinect") &&
			                                                    r.Culture.Name.Equals("en-US",
			                                                                          StringComparison.OrdinalIgnoreCase));
			// Check if list contains any matches
			var kinectMatchesList = kinectMatches as IList<RecognizerInfo> ?? kinectMatches.ToList();
			if (kinectMatchesList.Any())
				return kinectMatchesList.First();

			// Match first available
			return recognizersAvailable.First();
		}

		private static string GetSpeechSynthesizerVoice() {
			var installedVoices = SpeechProvider.speechSynthesizer.GetInstalledVoices();

			if (installedVoices.Count == 0)
				return String.Empty;
			
			// Return Zira if available
			if (installedVoices.Any(v => v.VoiceInfo.Name == "Microsoft Zira Desktop"))
				return "Microsoft Zira Desktop";
			
			// Return first available
			return installedVoices.First().VoiceInfo.Name;
		}


		#region Properties

		public static bool IsRunning { get; private set; }
		public static double SpeechRecognitionConfidence { get; set; }

		public static event SpeechProviderRecognizerEventHandler OnRecognitionHypothesized;
		public static event SpeechProviderRecognizerEventHandler OnRecognitionRejected;
		public static event SpeechProviderRecognizerEventHandler OnRecognitionAccepted;
		public static event SpeechProviderRecognizerEventHandler OnRecognitionDetected;

		public static event SpeechProviderSynthesizerEventHandler OnSynthesisRequested;
		public static event SpeechProviderSynthesizerEventHandler OnSynthesisStarted;
		public static event SpeechProviderSynthesizerEventHandler OnSynthesisEnded;

		#endregion
	}
}
