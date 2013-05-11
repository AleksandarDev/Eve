using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eve.Diagnostics.Logging;
using EveWindowsPhone.Adapters;
using EveWindowsPhone.Modules;
using EveWindowsPhone.ViewModels;

namespace EveWindowsPhone.Pages.Modules.Play {
	[Module("MPlay", "Play",
		"/Resources/Images/IPod.png",
		"/Pages/Modules/Play/PlayView.xaml")]
	public class PlayViewModel : NotificationObject {
		private readonly Log.LogInstance log =
			new Log.LogInstance(typeof(PlayViewModel));

		private readonly INavigationServiceFacade navigationServiceFacade;
		private readonly IIsolatedStorageServiceFacade isolatedStorageServiceFacade;
		private readonly IRelayServiceFacade relayServiceFacade;


		public PlayViewModel(INavigationServiceFacade navigationServiceFacade,
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
	}
}
