using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveWindowsPhone.Adapters;
using EveWindowsPhone.Modules;
using EveWindowsPhone.ViewModels;

namespace EveWindowsPhone.Pages.Modules.Touch {
	[Module("Touch", "/Resources/Images/Touch screens.png", typeof(TouchView))]
	public class TouchViewModel : NotificationObject {
		private INavigationServiceFacade navigationServiceFacade;


		public TouchViewModel(INavigationServiceFacade navigationServiceFacade) {
			if (navigationServiceFacade == null) throw new ArgumentNullException("navigationServiceFacade");
			this.navigationServiceFacade = navigationServiceFacade;
		}
	}
}
