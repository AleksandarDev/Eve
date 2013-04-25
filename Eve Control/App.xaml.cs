using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO.Ports;
using Eve.Diagnostics.Logging;
using MahApps.Metro;

namespace EveControl {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
		private const string EveGreenDictionaryPath =
			"/Eve.UI;component/Styles/Accents/EveGreen.xaml";

		/// <summary>
		/// Object constructor
		/// Runs initialization asynchronously 
		/// </summary>
		public App() {
			Task.Run(() => this.Initialize());
		}

		/// <summary>
		/// Initializes all application wide objects
		/// </summary>
		private void Initialize() {
			// Set application theme
			var eveGreenDictionary = new Uri(EveGreenDictionaryPath,
											 UriKind.RelativeOrAbsolute);
			var eveGreenAccent = new Accent("EveGreen", eveGreenDictionary);
			ThemeManager.ChangeTheme(this, eveGreenAccent, Theme.Dark);

			// Enable logging
			Log.Enabled = true;
			Log.WriteToDebug = true;
			Log.WriteToDebugLevel = Log.LogLevels.All;
		}
	}
}
