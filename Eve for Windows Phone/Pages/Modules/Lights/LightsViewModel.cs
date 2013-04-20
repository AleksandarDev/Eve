using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveWindowsPhone.Adapters;
using EveWindowsPhone.Modules;
using EveWindowsPhone.ViewModels;

namespace EveWindowsPhone.Pages.Modules.Lights {
	[Module("MLights", "Lights", "/Resources/Images/Light Bulb.png", "/Pages/Modules/Lights/LightsView.xaml")]
	public class LightsViewModel : NotificationObject {
		private readonly INavigationServiceFacade navigationServiceFacade;
		private readonly IIsolatedStorageServiceFacade isolatedStorageServiceFacade;


		public LightsViewModel(INavigationServiceFacade navigationServiceFacade,
		                       IIsolatedStorageServiceFacade isolatedStorageServiceFacade) {
			this.navigationServiceFacade = navigationServiceFacade;
			this.isolatedStorageServiceFacade = isolatedStorageServiceFacade;
		}
	}
}
