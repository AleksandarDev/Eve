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

			var relay = new RelayClient();
			relay.ConnectionChanged += client => this.StatusLabel.Dispatcher.InvokeAsync(
				() => {
					this.StatusLabel.Content = relay.IsConnected
												   ? "Connected"
												   : "Not connected";
				});

			relay.OnOpened += relayClient => {
				relay.Proxy.Subscribe();
				var clients = relay.Proxy.GetAvailableClients();
				foreach (var client in clients)
					System.Diagnostics.Debug.WriteLine(client.ID);
				System.Diagnostics.Debug.WriteLine("Subscribed to relay service");
			};
			this.Closing += (s, se) => relay.Close();
		}

		public delegate void RelayClientEventHandler(RelayClient client);

		public class RelayClient {
			private Log.LogInstance log = new Log.LogInstance(typeof(RelayClient));

			private InstanceContext instanceContext;
			private RelayServiceCallbackHandler callbackHandler;
			private Timer timer;
			private const int ConnectionCheckInterval = 5000;

			public event RelayClientEventHandler OnOpened;
			public event RelayClientEventHandler OnClosed;
			public event RelayClientEventHandler ConnectionChanged;


			public RelayClient() {
				this.callbackHandler = new RelayServiceCallbackHandler();
				this.instanceContext = new InstanceContext(this.callbackHandler);
				this.Proxy = new RelayServiceClient(this.instanceContext);

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
					System.Diagnostics.Debug.WriteLine("Already opening...",
													   (typeof(RelayClient)).Name);
					return;
				}

				// Check if connection is in faulted state so that we
				// abort all operations on proxy and get it to closed state
				if (this.Proxy.State == CommunicationState.Faulted) {
					System.Diagnostics.Debug.WriteLine(
						"Connection is faulted! Aborting and restarting.",
						(typeof(RelayClient)).Name);
					await Task.Run(() => this.Proxy.Abort());
				}

				// Open connection to relay
				System.Diagnostics.Debug.WriteLine("Opening connection...",
												   (typeof(RelayClient)).Name);
				await Task.Run(() => {
					try {
						this.Proxy.Open();
						log.Info("Connection opened");
						if (this.OnOpened != null)
							this.OnOpened(this);
					}
					catch (TimeoutException ex) {
						log.Error<TimeoutException>(ex, "Connection to proxy timed out!");
					} catch (Exception ex) {
						log.Error<Exception>(ex, "Unknown error occurred while connecting to proxy");
					}
				});
			}

			public async Task<bool> Close() {
				return await Task.Run(() => {
					// Check if connection isn't already closed
					if (this.Proxy.State != CommunicationState.Closed) {
						try {
							log.Info("Closing connection...");
							this.Proxy.Close();
							log.Info("Connection closed...");
						}
						catch (Exception ex) {
							log.Error<Exception>(ex, "Can't close connection!");
							return false;
						}
					}
					else log.Info("Connection already closed");

					return true;
				});
			}

			private async void CheckConnection(object sender, ElapsedEventArgs e) {
				System.Diagnostics.Debug.WriteLine("Checking connection...");

				if (this.Proxy.State != CommunicationState.Opened) {
					System.Diagnostics.Debug.WriteLine(
						"Connection closed. Requesting to open...");
					this.IsConnected = false;
					this.Open();
					return;
				}

				try {
					await this.Proxy.PingAsync("ConnectionCheck");
					System.Diagnostics.Debug.WriteLine("Connection confirmed");
					this.IsConnected = true;

					if (this.OnOpened != null)
						this.OnOpened(this);
				}
				catch (Exception) {
					System.Diagnostics.Debug.WriteLine("Connection to relay timed out",
													   (typeof(RelayClient)).Name);
					this.IsConnected = false;

					if (this.OnClosed != null)
						this.OnClosed(this);
				}
			}

			#region Properties

			public RelayServiceClient Proxy { get; private set; }
			public bool IsConnected { get; private set; }

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
