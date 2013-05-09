using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Eve.Core.Kinect;
using Eve.Diagnostics.Logging;
using Microsoft.Kinect;
using System.Speech.AudioFormat;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace Eve.API.Speech {
	[ProviderDescription("Speech Provider")]
	public class SpeechProvider : ProviderBase<SpeechProvider> {
		private SpeechRecognitionEngine speechRecognizer;
		private SpeechSynthesizer speechSynthesizer;
		private Stream audioSource;

		protected override void Initialize() {
			this.InitializeSpeechRecognizer();
			this.InitializeSpeechSynthesizer();
		}

		protected override void Uninitialize() {
			if (this.speechRecognizer != null) {
				this.speechRecognizer.SpeechRecognized -= SpeechRecognized;
				this.speechRecognizer.SpeechRecognitionRejected -= SpeechRejected;
				this.speechRecognizer.SpeechHypothesized -= SpeechHypothesized;
				this.speechRecognizer.SpeechDetected -= SpeechDetected;
				this.speechRecognizer.RecognizeAsyncCancel();
				this.speechRecognizer = null;
			}

			this.speechSynthesizer = null;

			if (this.audioSource != null)
				this.audioSource.Dispose();
			this.audioSource = null;
		}

		private void InitializeSpeechSynthesizer() {
			this.speechSynthesizer = new SpeechSynthesizer();
			this.speechSynthesizer.SetOutputToDefaultAudioDevice();
			string voice = this.GetSpeechSynthesizerVoice();
			if (voice != null)
				this.speechSynthesizer.SelectVoice(voice);
			this.speechSynthesizer.Rate = 0;

			this.log.Info("Speech Synthesizer created");
		}

		private void InitializeSpeechRecognizer() {
			// Get recognizer info
			var recognizerInfo = GetRecognizerInfo();
			if (recognizerInfo == null) {
				this.log.Error<Exception>(
					null, "No recognizer data found!");
				return;
			}

			// Setup recognizer
			this.speechRecognizer =
				new SpeechRecognitionEngine(recognizerInfo.Id);
			this.speechRecognizer.SpeechRecognized += SpeechRecognized;
			this.speechRecognizer.SpeechRecognitionRejected += SpeechRejected;
			this.speechRecognizer.SpeechHypothesized += SpeechHypothesized;
			this.speechRecognizer.SpeechDetected += SpeechDetected;

			// TODO Change between grammar and dictation
			// Create a grammar from grammar definition XML file.
			using (var stream = File.OpenRead("Speech/Grammars/EveGrammar.xml")) {
				var g = new Grammar(stream);
				this.speechRecognizer.LoadGrammar(g);
			}
			//this.speechRecognizer.LoadGrammar(
			//		new System.Speech.Recognition.DictationGrammar());

			// Get audio source
			// TODO Implement kinect selector from KinectProvider
			if (KinectService.HasSensorAvailable) {
				var kinectAudioSource = KinectService.GetAudioSource(0);
				if (kinectAudioSource == null) {
					this.log.Error<Exception>(
						null, "Can't get audio source for SpeechProvider!");
					return;
				}

				// Try to get Kinect as audio source
				kinectAudioSource.EchoCancellationMode =
					EchoCancellationMode.CancellationAndSuppression;
				kinectAudioSource.NoiseSuppression = true;
				kinectAudioSource.AutomaticGainControlEnabled = true;

				this.audioSource = kinectAudioSource.Start();

				// Set audio source as speech recognizer input stream
				if (this.audioSource != null && this.audioSource.CanRead)
					this.speechRecognizer.SetInputToAudioStream(
						this.audioSource, new SpeechAudioFormatInfo(EncodingFormat.Pcm,
							16000, 16, 1, 32000, 2, null));
			} else {
				// TODO Allow input device selection instead of just selecting default
				this.speechRecognizer.SetInputToDefaultAudioDevice();
			}

			// Initiate recognition
			this.speechRecognizer.RecognizeAsync(RecognizeMode.Multiple);

			this.log.Info("Speech Recognizer created");
		}

		// TODO Implement Speak synch
		public async Task SpeakAsync(SpeechPrompt speechPrompt) {
			if (!this.CheckIsRunning())
				return;

			if (this.OnSynthesisRequested != null)
				this.OnSynthesisRequested(
					new SpeechProviderSynthesizerEventArgs() {
						Synthesizer = this.speechSynthesizer,
						Prompt = speechPrompt
					});

			if (this.IsRunning && speechPrompt != null) {
				if (this.OnSynthesisStarted != null)
					this.OnSynthesisStarted(
						new SpeechProviderSynthesizerEventArgs() {
							Synthesizer = this.speechSynthesizer,
							Prompt = speechPrompt
						});

				// TODO Check if service needs to be stoped before synthesis
				//if (SpeechProvider.speechRecognizer != null) {
				//	System.Diagnostics.Debug.WriteLine("Stopping speech recognition...", typeof(SpeechProvider).Name);
				//	SpeechProvider.speechRecognizer.RecognizeAsyncCancel();
				//	System.Diagnostics.Debug.WriteLine("Speech recognition stopped", typeof(SpeechProvider).Name);
				//}

				System.Diagnostics.Debug.WriteLine("Speech request executing...",
					typeof(SpeechProvider).Name);
				await
					Task.Run(() => this.speechSynthesizer.Speak(speechPrompt.Prompt));
				System.Diagnostics.Debug.WriteLine("Speech request executed",
					typeof(SpeechProvider).Name);

				//if (SpeechProvider.speechRecognizer != null) {
				//	System.Diagnostics.Debug.WriteLine("Starting speech recognition...", typeof(SpeechProvider).Name);
				//	SpeechProvider.speechRecognizer.RecognizeAsync(RecognizeMode.Multiple);
				//	System.Diagnostics.Debug.WriteLine("Speech recognition started", typeof(SpeechProvider).Name);
				//}

				if (this.OnSynthesisEnded != null)
					this.OnSynthesisEnded(new SpeechProviderSynthesizerEventArgs() {
						Synthesizer = this.speechSynthesizer,
						Prompt = speechPrompt
					});
			}
		}

		private void SpeechDetected(object sender, SpeechDetectedEventArgs e) {
			this.log.Info("Speech detected...");

			if (this.OnRecognitionDetected != null)
				this.OnRecognitionDetected(
					new SpeechProviderRecognozerEventArgs() {
						Recognizer = this.speechRecognizer
					});
		}

		private void SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e) {
			this.log.Info("Hypothesized \"{0}\"", e.Result.Text);

			if (this.OnRecognitionHypothesized != null)
				this.OnRecognitionHypothesized(
					new SpeechProviderRecognozerEventArgs() {
						Recognizer = this.speechRecognizer,
						Result = e.Result
					});
		}

		private void SpeechRejected(object sender,
			SpeechRecognitionRejectedEventArgs e) {
			this.log.Info("Rejected \"{0}\"", e.Result.Text);

			if (this.OnRecognitionRejected != null)
				this.OnRecognitionRejected(
					new SpeechProviderRecognozerEventArgs() {
						Recognizer = this.speechRecognizer,
						Result = e.Result
					});
		}

		private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e) {
			if (e.Result.Confidence >= this.SpeechRecognitionConfidence) {
				this.log.Info("Accepted \"{0}\" for {1}%",
					e.Result.Text, e.Result.Confidence * 100);

				if (this.OnRecognitionAccepted != null)
					this.OnRecognitionAccepted(
						new SpeechProviderRecognozerEventArgs() {
							Recognizer = this.speechRecognizer,
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
			// TODO Implement recognizer selection 

			var recognizersAvailable = SpeechRecognitionEngine.InstalledRecognizers();

			// Check if any recognizer available
			if (recognizersAvailable.Count == 0)
				return null;

			// Match to Kinect for en-US
			var kinectMatches =
				recognizersAvailable.Where(
					r => r.AdditionalInfo.Values.Contains("Kinect") &&
						r.Culture.Name.Equals("en-US",
							StringComparison.OrdinalIgnoreCase));

			// Check if list contains any matches
			var kinectMatchesList =
				kinectMatches as IList<RecognizerInfo> ?? kinectMatches.ToList();
			if (kinectMatchesList.Any())
				return kinectMatchesList.First();

			// Match first available
			return recognizersAvailable.First();
		}

		private string GetSpeechSynthesizerVoice() {
			// TODO Implement voice selection

			ReadOnlyCollection<InstalledVoice> installedVoices = null;
			try {
				installedVoices = this.speechSynthesizer.GetInstalledVoices();
			}
			catch (PlatformNotSupportedException ex) {
				this.log.Error<PlatformNotSupportedException>(
					ex, "No voices installed");
				return null;
			}

			if (installedVoices.Count == 0)
				return String.Empty;

			// Return Zira if available
			if (installedVoices.Any(v => v.VoiceInfo.Name == "Microsoft Zira Desktop"))
				return "Microsoft Zira Desktop";

			// Return first available
			return installedVoices.First().VoiceInfo.Name;
		}

		#region Properties

		/// <summary>
		/// Gets or sets speech recognition confidence ratio
		/// This determines whether recorded and hypothesized elements gets accepted
		/// </summary>
		public double SpeechRecognitionConfidence { get; set; }

		public event SpeechProviderRecognizerEventHandler OnRecognitionHypothesized;
		public event SpeechProviderRecognizerEventHandler OnRecognitionRejected;
		public event SpeechProviderRecognizerEventHandler OnRecognitionAccepted;
		public event SpeechProviderRecognizerEventHandler OnRecognitionDetected;

		public event SpeechProviderSynthesizerEventHandler OnSynthesisRequested;
		public event SpeechProviderSynthesizerEventHandler OnSynthesisStarted;
		public event SpeechProviderSynthesizerEventHandler OnSynthesisEnded;

		#endregion
	}
}
