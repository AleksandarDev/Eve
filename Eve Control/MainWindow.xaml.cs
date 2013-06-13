﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Media;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Eve.API;
using Eve.API.Process;
using Eve.API.Scripting;
using Eve.API.Speech;
using Eve.API.Text;
using Eve.API.Touch;
using Eve.API.Vision;
using Eve.API.Chrome;
using Eve.Core.Kinect;
using Eve.Diagnostics.Logging;
using EveControl.Communication;
using EveControl.RelayServiceReference;
using EveControl.Windows.Chrome;
using EveControl.Windows.FaceController;
using EveControl.Windows.FridgeManager;
using EveControl.Windows.Log;
using EveControl.Windows.MainWindow;
using EveControl.Windows.Vision;
using Fleck2;
using Fleck2.Interfaces;
using MahApps.Metro;
using MahApps.Metro.Controls;

namespace EveControl {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow {
		// TODO Open magnifier when using touch option
		// TODO Chrome server to provider

		private readonly Log.LogInstance log = new Log.LogInstance(typeof(MainWindow));


		public MainWindow() {
			InitializeComponent();

			// Assign ViewModel
			if (this.ViewModel == null)
				if ((this.ViewModel = this.DataContext as MainViewModel) == null)
					throw new NullReferenceException("Invalid ViewModel");
		}

		private async void MainWindowOnLoaded(object sender, RoutedEventArgs e) {
			if (!ProviderManager.IsInitialized)
				ProviderManager.OnInitialized += async () => await this.MainWindowOnProvidersInitializedAsync();
			else await this.MainWindowOnProvidersInitializedAsync();
		}

		private async Task MainWindowOnProvidersInitializedAsync() {
			// Attach to speech provider events
			ProviderManager.SpeechProvider.OnStarted += async p => {
				// Say hallo when started
				await ProviderManager.SpeechProvider.SpeakAsync(
					new SpeechPrompt("Hello! I'm Eve, what can I do for you?"));
			};
			ProviderManager.SpeechProvider.OnRecognitionAccepted +=
				args => this.ViewModel.SpeechMessage = args.Result.Text;
			ProviderManager.SpeechProvider.OnRecognitionHypothesized +=
				args => this.ViewModel.SpeechMessage = args.Result.Text;
			ProviderManager.SpeechProvider.OnRecognitionRejected +=
				args => this.ViewModel.SpeechMessage = String.Empty;

			//ProviderManager.SpeechProvider.OnRecognitionAccepted += args => {
			//	if (args.Result.Semantics.Value.ToString() == "NextSong") {
			//		foreach (var connection in this.connections) {
			//			connection.Send("grooveshark:next-song");
			//		}
			//	}
			//	else if (args.Result.Semantics.Value.ToString() == "PreviousSong") {
			//		foreach (var connection in this.connections) {
			//			connection.Send("grooveshark:prev-song");
			//		}
			//	}
			//	else if (args.Result.Semantics.Value.ToString() == "PauseSong") {
			//		foreach (var connection in this.connections) {
			//			connection.Send("grooveshark:pause-song");
			//		}
			//	}
			//	else if (args.Result.Semantics.Value.ToString() == "ResumeSong") {
			//		foreach (var connection in this.connections) {
			//			connection.Send("grooveshark:resume-song");
			//		}
			//	}
			//	else if (args.Result.Semantics.Value.ToString() == "Computer") {
			//		var player = new SoundPlayer("Speech/Sounds/ComputerBegin.wav");
			//		player.Play();
			//	}
			//	else if (args.Result.Semantics.Value.ToString() == "TurnOnAmbientalLights") {
			//		SerialPort port = new SerialPort("COM28", 57600);
			//		try {
			//			port.Open();
			//			System.Diagnostics.Debug.WriteLine("Port open");
			//			byte[] buffer = new byte[] { 255, 0, 255 };
			//			port.Write(buffer, 0, buffer.Length);
			//			port.Close();
			//		}
			//		catch (Exception) {
			//			System.Diagnostics.Debug.WriteLine("Can't open port COM28");
			//		}
			//	}
			//	else if (args.Result.Semantics.Value.ToString() == "TurnOffAmbientalLights") {
			//		SerialPort port = new SerialPort("COM28", 57600);
			//		try {
			//			port.Open();
			//			System.Diagnostics.Debug.WriteLine("Port open");
			//			byte[] buffer = new byte[] { 0, 0, 0 };
			//			port.Write(buffer, 0, buffer.Length);
			//			port.Close();
			//		}
			//		catch (Exception) {
			//			System.Diagnostics.Debug.WriteLine("Can't open port COM28");
			//		}
			//	}
			// 
			//  // Clear text after 3 seconds
			//	this.Dispatcher.BeginInvoke(new Action(() => {
			//		this.EveVoiceRecognition.Text = args.Result.Text;
			//	}));
			//	System.Timers.Timer timer = new System.Timers.Timer();
			//	timer.Interval = 3000;
			//	timer.Elapsed += (s, es) => {
			//		timer.Stop();
			//		this.Dispatcher.BeginInvoke(new Action(() => {
			//			this.EveVoiceRecognition.Text = String.Empty;
			//		}));
			//	};
			//	timer.Start();
			//};

			// Start providers
			this.ViewModel.StatusMessage = "Starting service...";
			await this.ViewModel.InitializeServices();

			this.ViewModel.StatusMessage = "Connecting to relay service...";
			await this.ViewModel.InitializeConnection();

			this.ViewModel.StatusMessage = "Ready";
		}

		private void VisionViewOnClick(object sender, RoutedEventArgs e) {
			(new VisionView()).Show();
		}

		private async void MainWindowClosing(object sender, CancelEventArgs e) {
			if (ProviderManager.IsStarted)
				e.Cancel = true;

			// TODO close connection on window closing
			this.ViewModel.Dispose();
			await ProviderManager.StopAsync();

			this.Close();
		}

		private void FaceControllerViewOnClick(object sender, RoutedEventArgs e) {
			(new FaceControllerView()).Show();
		}

		private void FridgeManagerViewOnClick(object sender, RoutedEventArgs e) {
			(new FridgeManagerView()).Show();
		}

		private void ChromeViewOnClick(object sender, RoutedEventArgs e) {
			(new ChromeView()).Show();
		}

		#region Properties

		public MainViewModel ViewModel { get; private set; }

		#endregion
	}
}
