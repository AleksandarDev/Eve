using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Media;
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
using Eve.API.Scripting;
using Eve.API.Services.Common;
using Eve.API.Speech;
using Eve.API.Touch;
using Eve.API.Vision;
using Eve.Core.Chrome;
using Eve.Core.Kinect;
using Eve.Core.Loging;
using EveControl.Communication;
using EveControl.RelayServiceReference;
using Eve_Control.Windows.Vision;
using Fleck2;
using Fleck2.Interfaces;
using MahApps.Metro;
using MahApps.Metro.Controls;

namespace Eve_Control {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow {
		private readonly Log.LogInstance log = new Log.LogInstance(typeof(MainWindow));
		private RelayProxy relay;
		private RelayServiceCallbackHandler callbackHandler;
		private ServiceClient serviceClientData;
		private ChromeServer chromeServer;


		public MainWindow() {
			InitializeComponent();
		}

		private async void MainWindowOnLoaded(object sender, RoutedEventArgs e) {
			//await KinectService.Start();
			//await SpeechProvider.Start();
			//await TouchProvider.Start();
			//await ScriptingProvider.Start();
			this.Closing += async (s, es) => { await StopServices(); };

			System.Diagnostics.Debug.WriteLine("All Services started...", "Eve Control");

			//await SpeechProvider.Speak(new Prompt("Services initialized..."));
			//await SpeechProvider.Speak(new Prompt("Welcome!"));

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

			// Setup speech service
			//this.speechServiceHost = new ServiceHost(
			//	typeof (SpeechService), 
			//	new Uri("http://AleksandarPC:41250/"));
			//this.speechServiceHost.AddServiceEndpoint(
			//	typeof (Eve.API.Services.Speech.ISpeechService),
			//	new BasicHttpBinding(), "/API/Speech/");
			//this.speechServiceHost.Open();
			//System.Diagnostics.Debug.WriteLine("SpeechService opened.", typeof (MainWindow).Name);

			//this.touchServiceHost = new ServiceHost(
			//	typeof (TouchService),
			//	new Uri("http://AleksandarPC:41251/"));
			//this.touchServiceHost.AddServiceEndpoint(
			//	typeof (Eve.API.Services.Touch.ITouchService),
			//	new BasicHttpBinding(), "/API/Touch/");
			//this.touchServiceHost.Open();
			//System.Diagnostics.Debug.WriteLine("TouchService opened.", typeof (MainWindow).Name);

			this.chromeServer = new ChromeServer();
			System.Diagnostics.Debug.WriteLine(
				String.Format("WebSocket server started on \"{0}\"",
							  this.chromeServer.ServerLocation), typeof(MainWindow).Name);

			this.serviceClientData = new ServiceClient("Aleksandar Toplek Laptop",
													   "AleksandarPC");
			this.callbackHandler = new RelayServiceCallbackHandler();
			this.relay = new RelayProxy(callbackHandler);
			this.relay.ConnectionChanged += this.HandleRelayConnectionChanged;
			this.relay.OnOpened += async relayClient => {
				log.Info("Subscribing to relay service...");
				await relay.Relay.SubscribeAsync(this.serviceClientData);
				log.Info("Subscribed to relay service successful");
			};
			this.Closing += (s, se) => this.CloseRelayConnection();
			await this.relay.OpenAsync();
		}

		private void HandleRelayConnectionChanged(RelayProxy proxy) {
			// Change status label accordingly
			this.StatusLabel.Dispatcher.InvokeAsync(() =>
													this.StatusLabel.Content =
													proxy.IsConnected ? "Connected" : "Connecting...");
		}

		private async void CloseRelayConnection() {
			// Unsubscribe from service
			log.Info("Unsubscribing from relay service...");
			await relay.Relay.UnsibscribeAsync(this.serviceClientData);

			// Try closing service connection
			log.Info("Closing connection to relay service...");
			bool closedSuccessful = await relay.CloseAsync();
			if (!closedSuccessful) {
				// Force connection close to service
				log.Info("Forcing connection close to relay service...");
				closedSuccessful = await relay.CloseAsync(forceClose: true);
				if (!closedSuccessful)
					log.Error<Exception>(new Exception("Couldn't close connection"),
										 "Couldn't close connection to relay service!");
			}
			log.Info("Connection to relay service closed");
		}

		private async Task StopServices() {
			await ScriptingProvider.Stop();
			await SpeechProvider.Stop();
			await KinectService.Stop();
		}

		private void VisionViewOnClick(object sender, RoutedEventArgs e) {
			(new VisionView()).Show();
		}
	}
}
