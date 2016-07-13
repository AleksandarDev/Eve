using System.Threading.Tasks;
using System.Windows;
using Eve.Diagnostics.Logging;

namespace EveControl {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
		/// <summary>
		/// Object constructor
		/// Runs initialization asynchronously 
		/// </summary>
		public App() {
			// Close all windows on closing main window
			Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
	
			Task.Run(() => this.Initialize());
		}

		/// <summary>
		/// Initializes all application wide objects
		/// </summary>
		private void Initialize() {
			// Enable logging
			Log.Enabled = true;
			Log.WriteToDebug = true;
			Log.WriteToDebugLevel = Log.LogLevels.All;

			//this.Dispatcher.InvokeAsync(() => (new LogView()).Show());
		}
	}
}
