using System;
using System.Collections.Generic;
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
using Eve.API.Scripting;
using Eve.API.Speech;
using Eve.API.Touch;
using Eve.API.Vision;
using Eve.Core.Chrome;
using Eve.Core.Kinect;
using Eve.Core.Loging;
using Eve_Control.RelayServiceReference;
using Eve_Control.Windows.Vision;
using Fleck2;
using Fleck2.Interfaces;
using MahApps.Metro;
using MahApps.Metro.Controls;
using Timer = System.Timers.Timer;

namespace Eve_Control {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow {
		private Log.LogInstance log = new Log.LogInstance(typeof(MainWindow));
		private ChromeServer chromeServer;


		public MainWindow() {
			ThemeManager.ChangeTheme(this,
									 new Accent("EveGreen",
												new Uri("/Eve.UI;component/Styles/Accents/EveGreen.xaml",
														UriKind.RelativeOrAbsolute)), Theme.Dark);

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

			//System.Diagnostics.Debug.WriteLine("=== Creating Proxy...");

			//this.client = new RelayServiceClient(this.instanceContext,
			//									 new WSDualHttpBinding() {
			//										 ClientBaseAddress = new Uri("http://localhost:8088/RelayService/"),
			//										 Security = new WSDualHttpSecurity() { Mode = WSDualHttpSecurityMode.None }
			//									 },
			//									 new EndpointAddress("http://localhost:14004/RelayService.svc/Client/"));
			//System.Diagnostics.Debug.WriteLine("=== Opening connection...");
			//this.client.Open();
			//System.Diagnostics.Debug.WriteLine("=== Sending request...");
			//System.Diagnostics.Debug.WriteLine("=== Got message: " + this.client.Ping("Aleksandar"));
			//System.Diagnostics.Debug.WriteLine("=== Closing connectiontion");
			//this.client.Close();
			//await this.client.PingOneWayAsync("Aleksandar");

			this.relay = new RelayClient();
			this.relay.ConnectionChanged += this.HandleRelayConnectionChanged;
			this.relay.OnOpened += async relayClient => {
				log.Info("Subscribing to relay service...");
				await relay.Proxy.SubscribeAsync();
				log.Info("Subscribed to relay service successful");
			};
			this.Closing += (s, se) => this.CloseRelayConnection();
		}

		private RelayClient relay;

		private void HandleRelayConnectionChanged(RelayClient client) {
			// Change status label accordingly
			this.StatusLabel.Dispatcher.InvokeAsync(() =>
													this.StatusLabel.Content =
													client.IsConnected ? "Connected" : "Connecting...");
		}

		private async void CloseRelayConnection() {
			// Unsubscribe from service
			log.Info("Unsubscribing from relay service...");
			await relay.Proxy.UnsibscribeAsync();

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

		public delegate void RelayClientEventHandler(RelayClient client);

		public class RelayClient {
			// TODO Add firewall exception on initial connection timed out http://msdn.microsoft.com/en-us/library/aa366421(VS.85).aspx
			private Log.LogInstance log = new Log.LogInstance(typeof(RelayClient));

			private InstanceContext instanceContext;
			private RelayServiceCallbackHandler callbackHandler;
			private Timer timer;
			private bool isConnected;
			private const int ConnectionCheckInterval = 5000;
			private const string PingRequestContent = "Client";
			private const string PingResponsePrefix = "Hello ";

			public event RelayClientEventHandler OnOpened;
			public event RelayClientEventHandler OnClosed;
			public event RelayClientEventHandler ConnectionChanged;


			public RelayClient() {
				this.callbackHandler = new RelayServiceCallbackHandler();
				this.instanceContext = new InstanceContext(this.callbackHandler);
				this.Proxy = new RelayServiceClient(this.instanceContext);

				this.IsConnected = false;

				// Attach to open and close events
				this.OnOpened +=
					client => { if (this.ConnectionChanged != null) this.ConnectionChanged(client); };
				this.OnClosed +=
					client => { if (this.ConnectionChanged != null) this.ConnectionChanged(client); };

				// Set periodic connection tests
				this.timer = new Timer(ConnectionCheckInterval);
				this.timer.Elapsed += CheckConnection;
				this.timer.Start();

				this.Open();
			}


			private async void Open() {
				// Check if communication isn't opening already
				if (this.Proxy.State == CommunicationState.Opening) {
					this.log.Info("Already opening...");
					return;
				}

				// Check if connection is in faulted state so that we
				// abort all operations on proxy and get it to closed state
				if (this.Proxy.State == CommunicationState.Faulted) {
					this.log.Info("Connection is faulted! Aborting and restarting.");
					await Task.Run(() => this.Proxy.Abort());
				}

				// Open connection to relay
				this.log.Info("Opening connection...");
				await Task.Run(() => {
					try {
						this.Proxy.Open();
						this.log.Info("Connection opened");
						this.IsConnected = true;
					}
					catch (TimeoutException ex) {
						this.log.Error<TimeoutException>(ex, "Unable to open connection to proxy - timed out");
						this.IsConnected = false;
						// TODO Activate pooling
						// NOTE This could be due to firewall
					} catch (Exception ex) {
						this.log.Error<Exception>(ex, "Unknown error occurred while connecting to proxy");
						this.IsConnected = false;
					}
				});
			}

			public async Task<bool> CloseAsync(bool forceClose = false) {
				return await Task.Run(() => this.Close(forceClose));
			}

			private bool Close(bool forceClose = false) {
				// Check if connection isn't already closed
				if (this.Proxy.State == CommunicationState.Closed) {
					this.log.Info("Connection already closed");
					return true;
				}

				// Try to close connection
				try {
					if (forceClose) {
						log.Info("Aborting connection...");
						this.Proxy.Abort();
						log.Info("Connection aborted");
					}
					else {
						log.Info("Closing connection...");
						this.Proxy.Close();
						log.Info("Connection closed");
					}

					return true;
				} catch (Exception ex) {
					log.Error<Exception>(ex, "Unable close/abort connection!");
					return false;
				}
			}

			private async void CheckConnection(object sender, ElapsedEventArgs e) {
				this.log.Info("Checking connection...");

				if (this.Proxy.State != CommunicationState.Opened) {
					this.log.Warn("Connection closed. Requesting to reopen...");
					this.IsConnected = false;
					this.Open();
					return;
				}

				try {
					string response = await this.Proxy.PingAsync(PingRequestContent);
					if (response != PingResponsePrefix + PingRequestContent) {
						this.log.Warn("Ping responded with wrong data");
						this.IsConnected = false;
					}
					else {
						this.log.Info("Connection confirmed");
						this.IsConnected = true;
					}
				}
				catch (Exception) {
					this.log.Warn("Connection to relay timed out");
					this.IsConnected = false;
				}
			}

			#region Properties

			public RelayServiceClient Proxy { get; private set; }

			public bool IsConnected {
				get { return this.isConnected; }
				set {
					bool hasChanged = this.isConnected != value;
					this.isConnected = value;

					if (hasChanged && value) {
						if (this.OnOpened != null)
							this.OnOpened(this);
					}
					else if (hasChanged) {
						if (this.OnClosed != null)
							this.OnClosed(this);
					}
				}
			}

			#endregion

			public class RelayServiceCallbackHandler : IRelayServiceCallback {
				public string PingRequest(string message) {
					return "Control" + message;
				}
			}
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
