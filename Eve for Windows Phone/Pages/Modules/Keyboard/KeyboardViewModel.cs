using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eve.Diagnostics.Logging;
using EveWindowsPhone.Adapters;
using EveWindowsPhone.Modules;
using EveWindowsPhone.Pages.AdvancedSettings;
using EveWindowsPhone.ViewModels;

namespace EveWindowsPhone.Pages.Modules.Keyboard {
	[Module("MKeyboard", "Keyboard", 
		"/Resources/Images/Keyboard.png", 
		"/Pages/Modules/Keyboard/KeyboardView.xaml")]
	public class KeyboardViewModel : NotificationObject {
		private readonly Log.LogInstance log =
			new Log.LogInstance(typeof(KeyboardViewModel));

		private readonly INavigationServiceFacade navigationServiceFacade;
		private readonly IIsolatedStorageServiceFacade isolatedStorageServiceFacade;
		private readonly IRelayServiceFacade relayServiceFacade;


		public KeyboardViewModel(INavigationServiceFacade navigationServiceFacade,
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
		}

		public void AdvancedSettings() {
			AdvancedSettingsView.NavigateWithIndex(this.navigationServiceFacade, 3);
		}
	}
}
