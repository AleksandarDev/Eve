using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eve.Diagnostics.Logging;
using EveWindowsPhone.Adapters;
using EveWindowsPhone.Modules;
using EveWindowsPhone.ViewModels;

namespace EveWindowsPhone.Pages.Modules.Vision {
	[Module("MVision", "Vision",
		"/Resources/Images/",
		"/Pages/Modules/Vision/VisionView.xaml")]
	public class VisionViewModel : NotificationObject {
		private readonly Log.LogInstance log =
			new Log.LogInstance(typeof(VisionViewModel));

		private readonly INavigationServiceFacade navigationServiceFacade;
		private readonly IIsolatedStorageServiceFacade isolatedStorageServiceFacade;
		private readonly IRelayServiceFacade relayServiceFacade;


		public VisionViewModel(INavigationServiceFacade navigationServiceFacade,
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
