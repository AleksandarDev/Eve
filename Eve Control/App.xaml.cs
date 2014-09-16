using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO.Ports;
using Eve.Diagnostics.Logging;
using EveControl.Windows.Log;
using MahApps.Metro;

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
