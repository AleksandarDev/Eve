using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO.Ports;
using Eve.Core.Loging;

namespace Eve_Control {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
		public App() {
			Log.Enabled = true;
			//Log.WriteToDebug = true;
			Log.WriteToDebugLevel = Log.LogLevels.All;
		}
	}
}
