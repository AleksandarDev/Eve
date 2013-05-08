using System;
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
using Eve.API.Services.Common;
using Eve.API.Speech;
using Eve.API.Text;
using Eve.API.Touch;
using Eve.API.Vision;
using Eve.Core.Chrome;
using Eve.Core.Kinect;
using Eve.Diagnostics.Logging;
using EveControl.Communication;
using EveControl.RelayServiceReference;
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
		private RelayProxy relay;
		private RelayServiceCallbackHandler callbackHandler;
		private ServiceClient serviceClientData;
		private ChromeServer chromeServer;


		public MainWindow() {
			InitializeComponent();
		}

		private async void MainWindowOnLoaded(object sender, RoutedEventArgs e) {
			// SpeechProvider hello message
			ProviderManager.SpeechProvider.OnStarted += async p => {
				await ProviderManager.SpeechProvider.SpeakAsync(
					new SpeechPrompt("Speech provider started..."));
				await ProviderManager.SpeechProvider.SpeakAsync(
					new SpeechPrompt("Welcome!"));
			};

			// Start providers
			this.ShowCloseButton = false;
			await ProviderManager.StartAsync();
			this.ShowCloseButton = true;
			//SpeechProvider.OnRecognitionAccepted += args => {
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
			//SpeechProvider.OnRecognitionHypothesized += args => {
			//	this.Dispatcher.BeginInvoke(new Action(() => {
			//		this.EveVoiceRecognition.Text = args.Result.Text;
			//	}));
			//};
			//SpeechProvider.OnRecognitionRejected += args => {
			//	this.Dispatcher.BeginInvoke(new Action(() => {
			//		this.EveVoiceRecognition.Text = String.Empty;
			//	}));
			//};

			// Chrome server
			//this.chromeServer = new ChromeServer();
			//System.Diagnostics.Debug.WriteLine(
			//	String.Format("WebSocket server started on \"{0}\"",
			//				  this.chromeServer.ServerLocation), typeof(MainWindow).Name);

			// Relay proxy
			this.serviceClientData = new ServiceClient("Aleksandar Toplek Laptop",
													   "AleksandarPC");
			this.callbackHandler = new RelayServiceCallbackHandler();
			this.relay = new RelayProxy(callbackHandler);
			this.relay.ConnectionChanged += this.HandleRelayConnectionChanged;
			this.relay.OnOpened += async relayClient => {
				this.log.Info("Subscribing to relay service...");
				await relay.Relay.SubscribeAsync(this.serviceClientData);
				this.log.Info("Subscribed to relay service successful");
			};

			this.relay.OpenAsync();

			this.log.Info("Starting display zoom");
			ProviderManager.DisplayEnhancementsProvider.ZoomInitialSetup();
			await ProviderManager.DisplayEnhancementsProvider.SetZoom(200);
		}

		private void HandleRelayConnectionChanged(RelayProxy proxy) {
			// Change status label accordingly
			this.StatusLabel.Dispatcher.InvokeAsync(() =>
													this.StatusLabel.Content =
													proxy.IsConnected ? "Connected" : "Connecting...");
		}

		private async Task CloseRelayConnectionAsync() {
			if (this.relay == null) {
				this.log.Warn("Couldn't close connection. Relay not yet instanciated");
				return;
			}

			// Unsubscribe from service
			this.log.Info("Unsubscribing from relay service...");
			if (this.relay.Relay.State == CommunicationState.Opened)
				await this.relay.Relay.UnsibscribeAsync(this.serviceClientData);

			// Try closing service connection
			this.log.Info("Closing connection to relay service...");
			bool closedSuccessful = this.relay.Close();
			if (!closedSuccessful) {
				// Force connection close to service
				this.log.Info("Forcing connection close to relay service...");
				closedSuccessful = await this.relay.CloseAsync(forceClose: true);
				if (!closedSuccessful)
					this.log.Error<Exception>(new Exception("Couldn't close connection"),
										 "Couldn't close connection to relay service!");
			}
			this.log.Info("Connection to relay service closed");
		}

		private void VisionViewOnClick(object sender, RoutedEventArgs e) {
			(new VisionView()).Show();
		}

		private async void MainWindowClosing(object sender, CancelEventArgs e) {
			if (ProviderManager.IsStarted)
				e.Cancel = true;

			await this.CloseRelayConnectionAsync();
			await ProviderManager.StopAsync();

			this.Close();
		}
	}
}
