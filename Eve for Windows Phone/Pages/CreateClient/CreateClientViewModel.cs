using EveWindowsPhone.Adapters;
using EveWindowsPhone.ViewModels;

namespace EveWindowsPhone.Pages.ChangeClient {
	public class CreateClientViewModel : NotificationObject {
		private readonly INavigationServiceFacade navigationServiceFacade;
		private readonly IIsolatedStorageServiceFacade isolatedStorageServiceFacade;


		public CreateClientViewModel(INavigationServiceFacade navigationServiceFacade,
		                             IIsolatedStorageServiceFacade isolatedStorageServiceFacade) {
			this.navigationServiceFacade = navigationServiceFacade;
			this.isolatedStorageServiceFacade = isolatedStorageServiceFacade;
		}
	}
}