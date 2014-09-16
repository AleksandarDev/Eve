using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace EveServices {
	public partial class EveAPIService : ServiceBase {
		public EveAPIService() {
			InitializeComponent();
		}

		protected override void OnStart(string[] args) {
			System.Diagnostics.Debug.WriteLine("EveAPIService started.");
		}

		protected override void OnStop() {
			System.Diagnostics.Debug.WriteLine("EveAPIService stoped.");
		}
	}
}
