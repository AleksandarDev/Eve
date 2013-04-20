using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveWindowsPhone.Adapters;
using EveWindowsPhone.Modules;
using EveWindowsPhone.ViewModels;

namespace EveWindowsPhone.Pages.Modules.Play {
	[Module("MPlay", "Play", "/Resources/Images/IPod.png", "/Pages/Modules/Play/PlayView.xaml")]
	public class PlayViewModel : NotificationObject {
		private readonly INavigationServiceFacade navigationServiceFacade;
		private readonly IIsolatedStorageServiceFacade isolatedStorageServiceFacade;


		public PlayViewModel(INavigationServiceFacade navigationServiceFacade,
		                     IIsolatedStorageServiceFacade isolatedStorageServiceFacade) {
			this.navigationServiceFacade = navigationServiceFacade;
			this.isolatedStorageServiceFacade = isolatedStorageServiceFacade;
		}
	}
}
