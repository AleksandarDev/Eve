using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Eve.API;
using Eve.Diagnostics.Logging;
using EveControl.Windows.Chrome;
using EveControl.Windows.FaceController;
using EveControl.Windows.FridgeManager;
using EveControl.Windows.Log;
using EveControl.Windows.Main;
using EveControl.Windows.Vision;
using MahApps.Metro.Controls;

namespace EveControl {
	/// <summary>
	/// Interaction logic for MainView.xaml
	/// </summary>
	public partial class MainView : MetroWindow {
		// TODO Open magnifier when using touch option
		// TODO Chrome server to provider

		private readonly Log.LogInstance log = 
			new Log.LogInstance(typeof(MainView));


		public MainView() {
			InitializeComponent();

			// Assign ViewModel
			if (this.ViewModel == null)
				if ((this.ViewModel = this.DataContext as MainViewModel) == null)
					throw new NullReferenceException("Invalid ViewModel");
		}

		private void MainWindowOnLoaded(object sender, RoutedEventArgs e) {

		}

		private async Task MainWindowOnProvidersInitializedAsync() {
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
			//};
		}

		private async void MainWindowClosing(object sender, CancelEventArgs e) {
			if (ProviderManager.IsStarted)
				e.Cancel = true;

			// TODO close connection on window closing
			this.ViewModel.Dispose();
			await ProviderManager.StopAsync();

			this.Close();
		}

		private void LogViewOnClick(object sender, RoutedEventArgs e) {
			(new LogView()).Show();
		}

		private void VisionViewOnClick(object sender, RoutedEventArgs e) {
			(new VisionView()).Show();
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

		private void MainViewOnPreviewMouseMove(object sender, MouseEventArgs e) {
			if (e.LeftButton == MouseButtonState.Pressed)
				if (e.OriginalSource is Grid &&
					(e.OriginalSource as Grid).Parent is MetroContentControl)
					this.DragMove();
		}
        
		#region Properties

		public MainViewModel ViewModel { get; private set; }

		#endregion
	}
}
