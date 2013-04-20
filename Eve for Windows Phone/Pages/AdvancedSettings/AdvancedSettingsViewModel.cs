using EveWindowsPhone.Adapters;
using EveWindowsPhone.ViewModels;

namespace EveWindowsPhone.Pages.AdvancedSettings {
	public class AdvancedSettingsViewModel : NotificationObject {
		private readonly INavigationServiceFacade navigationServiceFacade;
		private readonly IIsolatedStorageServiceFacade isolatedStorageServiceFacade;

		private int favoriteRows;


		public AdvancedSettingsViewModel(INavigationServiceFacade navigationServiceFacade,
		                                 IIsolatedStorageServiceFacade isolatedStorageServiceFacade) {
			this.navigationServiceFacade = navigationServiceFacade;
			this.isolatedStorageServiceFacade = isolatedStorageServiceFacade;

			this.LoadSettings();
		}

		private void LoadSettings() {
			this.FavoriteRows = this.isolatedStorageServiceFacade.GetSetting<int>(IsolatedStorageServiceFacade.FavoriteRowsKey);
		}

		#region Properties

		public int FavoriteRows {
			get { return this.favoriteRows; }
			set {
				this.favoriteRows = value;
				RaisePropertyChanged("FavoriteRows");
				this.isolatedStorageServiceFacade.SetSetting(favoriteRows, IsolatedStorageServiceFacade.FavoriteRowsKey);
			}
		}

		#endregion
	}
}