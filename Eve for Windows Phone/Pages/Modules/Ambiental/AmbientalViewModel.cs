using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eve.Diagnostics.Logging;
using EveWindowsPhone.Adapters;
using EveWindowsPhone.Modules;
using EveWindowsPhone.RelayServiceReference;
using EveWindowsPhone.ViewModels;

namespace EveWindowsPhone.Pages.Modules.Ambiental {
	[Module("MAmbiental", "Ambiental",
		"/Resources/Images/Light-Bulb-Ambiental.png", 
		"/Pages/Modules/Ambiental/AmbientalView.xaml")]
	public class AmbientalViewModel : NotificationObject {
		private readonly Log.LogInstance log =
			new Log.LogInstance(typeof(AmbientalViewModel));

		private readonly INavigationServiceFacade navigationServiceFacade;
		private readonly IIsolatedStorageServiceFacade isolatedStorageServiceFacade;
		private readonly IRelayServiceFacade relayServiceFacade;

		private bool isLoadingLights;
		private List<AmbientalLight> lights;
 

		public AmbientalViewModel(INavigationServiceFacade navigationServiceFacade,
								  IIsolatedStorageServiceFacade isolatedStorageServiceFacade,
								  IRelayServiceFacade relayServiceFacade) {
			if (navigationServiceFacade == null)
				throw new ArgumentNullException("navigationServiceFacade");
			if (isolatedStorageServiceFacade == null)
				throw new ArgumentNullException("isolatedStorageServiceFacade");
			if (relayServiceFacade == null)
				throw new ArgumentNullException("relayServiceFacade");

			this.navigationServiceFacade = navigationServiceFacade;
			this.isolatedStorageServiceFacade = isolatedStorageServiceFacade;
			this.relayServiceFacade = relayServiceFacade;

			this.lights = new List<AmbientalLight>();
			this.IsLoadingLights = true;

			this.relayServiceFacade.Proxy.Relay.GetAmbientalLightsCompleted += RelayGetAmbientalLightsCompleted;
			this.relayServiceFacade.Proxy.Relay.GetAmbientalLightsAsync();
			this.log.Info("Requested for available lights list");
		}

		private void RelayGetAmbientalLightsCompleted(object sender, GetAmbientalLightsCompletedEventArgs e) {
			this.lights = e.Result.ToList();

			this.IsLoadingLights = false;

			string lightsList = String.Empty;
			this.lights.ForEach(l => lightsList += l.ToString() + "\n");
			this.log.Info("Got lights list:\n{0}", lightsList);
		}

		public IEnumerable<AmbientalLight> GetLights() {
			return this.lights;
		}


		#region Properties

		public bool IsLoadingLights {
			get { return this.isLoadingLights; }
			private set {
				this.isLoadingLights = value;
				this.RaisePropertyChanged("IsLoadingLights");
			}
		}

		#endregion
	}
}
