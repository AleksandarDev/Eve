using System;
using System.Collections.Generic;
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
using Eve.API.Vision;
using Eve.Core.Chrome;
using Eve.Core.Kinect;
using Fleck2;
using Fleck2.Interfaces;
using Timer = System.Timers.Timer;

namespace Eve_Control {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		private ChromeServer chromeServer;


		public MainWindow() {
			this.DataContext = this;
			InitializeComponent();
		}

		private async void MainWindowOnLoaded(object sender, RoutedEventArgs e) {
			//await KinectService.Start();
			await SpeechProvider.Start();
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
			this.speechServiceHost = new ServiceHost(
				typeof (Eve.API.Services.Speech.SpeechService), 
				new Uri("http://localhost:41250/"));
			this.speechServiceHost.AddServiceEndpoint(
				typeof (Eve.API.Services.Speech.ISpeechService),
				new WSHttpBinding(), "/API/Speech/");
			this.speechServiceHost.Open();
			System.Diagnostics.Debug.WriteLine("SpeechService opened.", typeof (MainWindow).Name);

			this.chromeServer = new ChromeServer();
			System.Diagnostics.Debug.WriteLine(String.Format("WebSocket server started on \"{0}\"",
			                                                 this.chromeServer.ServerLocation), typeof (MainWindow).Name);

			var faceProvider = new FaceDetectionProvider();
		}

		private ServiceHost speechServiceHost; 

		private async Task StopServices() {
			await ScriptingProvider.Stop();
			await SpeechProvider.Stop();
			await KinectService.Stop();
		}

		#region Window handling

		// Window handling variables
		private bool isMaximized;
		private double restoresHeight, restoredWidth, restoredTop, restoredLeft;

		/// <summary>
		/// Method called upon template apply on this object
		/// </summary>
		public override void OnApplyTemplate() {
			// Attach event for window drag move
			Rectangle windowMoveRect = this.GetObjectFromTemplate<Rectangle>("NewUIWindowHeaderMoveRectangle");
			if (windowMoveRect != null)
				windowMoveRect.PreviewMouseLeftButtonDown += WindowMove;
			else throw new NullReferenceException("windowMoveRect");

			// Attach event for window minimize button
			Button minimizeButton = this.GetObjectFromTemplate<Button>("NewUIWindowHeaderMinimizeButton");
			if (minimizeButton != null)
				minimizeButton.Click += WindowMinimize;
			else throw new NullReferenceException("minimizeButton");

			// Disable window maximize/restore button
			Button maximizeButton = this.GetObjectFromTemplate<Button>("NewUIWindowHeaderMaximizeButton");
			if (maximizeButton != null)
				maximizeButton.Click += WindowMaximize;
			else throw new NullReferenceException("maximizeButton");

			// Attach event for window close button
			Button closeButton = this.GetObjectFromTemplate<Button>("NewUIWindowHeaderCloseButton");
			if (closeButton != null)
				closeButton.Click += WindowClose;
			else throw new NullReferenceException("closeButton");


			// Set Window icon image
			var windowIcon = this.GetObjectFromTemplate<Image>("NewUIWindowHeaderIcon");
			if (windowIcon != null)
				windowIcon.Source =
					new BitmapImage(new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/Images/Eve/Eve48.png",
					                        UriKind.Absolute));
			else throw new NullReferenceException("windowIcon");

			base.OnApplyTemplate();
		}


		/// <summary>
		/// Closes window
		/// </summary>
		/// <param name="sender">Not used</param>
		/// <param name="e">Not used</param>
		private void WindowClose(object sender, RoutedEventArgs e) {
			this.Close();
		}

		/// <summary>
		/// Sets window state to minimized
		/// </summary>
		/// <param name="sender">Not used</param>
		/// <param name="e">Not used</param>
		private void WindowMinimize(object sender, RoutedEventArgs e) {
			this.WindowState = WindowState.Minimized;
		}

		/// <summary>
		/// Maximizes/Restores window
		/// </summary>
		/// <param name="sender">Not used</param>
		/// <param name="e">Not used</param>
		void WindowMaximize(object sender, RoutedEventArgs e) {
			Button windowStateButton = this.GetObjectFromTemplate<Button>("NewUIWindowHeaderMaximizeButton");
			if (windowStateButton == null)
				throw new NullReferenceException("windowStateButton");

			if (!this.isMaximized) {
				windowStateButton.Content = "2";

				this.restoredTop = this.Top;
				this.restoredLeft = this.Left;
				this.restoredWidth = this.Width;
				this.restoresHeight = this.Height;

				var border = this.GetObjectFromTemplate<Border>("NewUIWindowBorder");
				if (border != null) {
					border.BorderThickness = new Thickness(0);
				}

				this.Top = this.Left = 0;
				this.Height = SystemParameters.PrimaryScreenHeight;
				this.Width = SystemParameters.PrimaryScreenWidth;

				this.isMaximized = true;
			}
			else {
				windowStateButton.Content = "1";

				this.Top = this.restoredTop;
				this.Left = this.restoredLeft;
				this.Height = this.restoresHeight;
				this.Width = this.restoredWidth;

				var border = this.GetObjectFromTemplate<Border>("NewUIWindowBorder");
				if (border != null) {
					border.BorderThickness = new Thickness(1);
				}

				this.isMaximized = false;
			}
		}

		/// <summary>
		/// Starts drag move action on window
		/// </summary>
		/// <param name="sender">Not used</param>
		/// <param name="e">Event arguments for mouse clikc</param>
		private void WindowMove(object sender, MouseButtonEventArgs e) {
			if (e.ClickCount == 2)
				this.WindowMaximize(null, null);
			else this.DragMove();
		}

		/// <summary>
		/// Finds dependency object of given name in template and casts it to requested type
		/// </summary>
		/// <typeparam name="T">Type to cast dependency object to</typeparam>
		/// <param name="name">Name of object in template</param>
		/// <returns>Returns matching object from template or null if object of requested type isn't found</returns>
		/// <exception cref="ArgumentNullException">Argument "name" is null</exception>
		private T GetObjectFromTemplate<T>(string name) where T : FrameworkElement {
			if (String.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			return this.GetTemplateChild(name) as T;
		}

		#endregion
	}
}
