using System.ServiceProcess;

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
