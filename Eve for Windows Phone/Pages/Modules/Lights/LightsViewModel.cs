using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eve.Diagnostics.Logging;
using EveWindowsPhone.Adapters;
using EveWindowsPhone.Modules;
using EveWindowsPhone.Pages.AdvancedSettings;
using EveWindowsPhone.RelayServiceReference;
using EveWindowsPhone.ViewModels;

namespace EveWindowsPhone.Pages.Modules.Lights {
	[Module("MLights", "Lights", 
		"/Resources/Images/Light Bulb.png",
		"/Pages/Modules/Lights/LightsView.xaml", 
		isInternal: false, isEnabled: true)]
	public class LightsViewModel : NotificationObject {
		private readonly Log.LogInstance log =
			new Log.LogInstance(typeof(LightsViewModel));

		private readonly INavigationServiceFacade navigationServiceFacade;
		private readonly IIsolatedStorageServiceFacade isolatedStorageServiceFacade;
		private readonly IRelayServiceFacade relayServiceFacade;

		private bool isLoadingLights;
		public event LightsViewModelEventHandler OnLightListRefreshed;


		public LightsViewModel(INavigationServiceFacade navigationServiceFacade,
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

			// Set initial values
			this.Lights = new List<Light>();
			this.IsLoadingLights = true;

			// Initiate first lights list request
			this.RefreshLightsListAsync();
		}

		private void RelayGetLightsCompleted(object sender, GetLightsCompletedEventArgs e) {
			// Detach event handler
			this.relayServiceFacade.Proxy.Relay.GetLightsCompleted -= RelayGetLightsCompleted;

			if (e.Result == null) {
				this.log.Warn("Couldn't retrieve lights");
				this.Lights.Clear();
				this.IsLoadingLights = true;
				// TODO Inform user
			}
			else {
				// Retrieve result
				this.Lights = e.Result.ToList();

				// Set as loaded
				this.IsLoadingLights = false;

				string lightsList = String.Empty;
				this.Lights.ForEach(
					l =>
					lightsList +=
					String.Format("\n{0} [{1}]: {2}", l.Alias, l.ID, l.State ? "On" : "Off"));
				this.log.Info("Got lights list: {0}", lightsList);
			}

			// Call list refreshed event if needed
			if (this.OnLightListRefreshed != null)
				this.OnLightListRefreshed(this);
		}

		public void RefreshLightsListAsync() {
			this.relayServiceFacade.Proxy.Relay.GetLightsCompleted += RelayGetLightsCompleted;
			this.relayServiceFacade.Proxy.Relay.GetLightsAsync(
				this.relayServiceFacade.Proxy.ActiveDetails);

			this.log.Info("Requested for available lights list");
		}

		public void SwitchLight(int id, bool? isChecked) {
			// Check if given state value isn't null
			if (!isChecked.HasValue) {
				this.log.Warn("No switch value given for [{0}]", id);
				return;
			}

			// Check if there is any light available
			if (!this.Lights.Any()) {
				this.log.Warn("No lights available to switch");
				return;
			}

			// Check if light exists
			var light = this.Lights.First(l => l.ID == id);
			if (light == null) {
				this.log.Warn("Light with given id [{0}] not found!", id);
				return;
			}

			// Check if we need to change state
			if (light.State != isChecked.Value) {
				this.relayServiceFacade.Proxy.Relay.SetLightStateAsync(
					this.relayServiceFacade.Proxy.ActiveDetails, id, isChecked.Value);

				this.log.Info("Sending light [{0}] state request [{0}]",
							  id, isChecked.Value ? "On" : "Off");
			}
		}

		#region Properties

		public bool IsLoadingLights {
			get { return this.isLoadingLights; }
			private set {
				this.isLoadingLights = value;
				this.RaisePropertyChanged(() => this.IsLoadingLights);
			}
		}

		public List<Light> Lights { get; private set; }

		#endregion

		public void AdvancedSettings() {
			AdvancedSettingsView.NavigateWithIndex(this.navigationServiceFacade, 4);
		}
	}
}
