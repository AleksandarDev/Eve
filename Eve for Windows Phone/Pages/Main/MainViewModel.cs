using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using Eve.Diagnostics.Logging;
using EveWindowsPhone.Adapters;
using EveWindowsPhone.Modules;
using EveWindowsPhone.ViewModels;

namespace EveWindowsPhone.Pages.Main {
	public class MainViewModel : NotificationObject {
		private readonly Log.LogInstance log =
			new Log.LogInstance(typeof(MainViewModel));

		private readonly INavigationServiceFacade navigationServiceFacade;
		private readonly IIsolatedStorageServiceFacade isolatedStorageServiceFacade;

		private bool isEditingFavorites;
		private int tileRows;


		public MainViewModel(INavigationServiceFacade navigationServiceFacade,
			IIsolatedStorageServiceFacade isolatedStorageServiceFacade) {
			if (navigationServiceFacade == null)
				throw new ArgumentNullException("navigationServiceFacade");
			if (isolatedStorageServiceFacade == null)
				throw new ArgumentNullException("isolatedStorageServiceFacade");

			this.navigationServiceFacade = navigationServiceFacade;
			this.isolatedStorageServiceFacade = isolatedStorageServiceFacade;

			this.FavoriteModules = new ObservableCollection<ModuleModel>();
			this.AvailableModules = new ObservableCollection<ModuleModel>();

			this.LoadSettings();
			this.LoadModules();

			this.log.Info("View model created");
		}


		public void LoadSettings() {
			this.tileRows = this.isolatedStorageServiceFacade.GetSetting<int>(
				IsolatedStorageServiceFacade.FavoriteRowsKey);
		}

		private void LoadModules() {
			// Get modules locator object
			var modulesLocator =
				Application.Current.Resources["ModulesLocator"] as ModulesLocator;
			if (modulesLocator == null)
				throw new NullReferenceException("Can't find ModulesLocator");

			this.AvailableModules = modulesLocator.AvailableModules;

			this.LoadFavoriteModules();
		}

		private void LoadFavoriteModules() {
			this.FavoriteModules.Clear();

			try {
				// Retrieve modules list if available
				var savedFavorites = this.isolatedStorageServiceFacade.GetFavoriteModules();
				this.log.Info("{0} saved favorites retrieved", savedFavorites.Modules.Count);

				// Add each module to the favorite list
				foreach (var module in savedFavorites.Modules) {
					var available = this.AvailableModules.First(m => m.Module.ID == module.ID);
					available.IsFavorite = true;
					this.FavoriteModules.Add(available);
				}
			}
			catch (InvalidOperationException) {
				this.log.Info("No favorite modules saved");
			}
		}

		public void SetModulesEditMode(bool isEditing) {
			// Set all modules to edit mode
			foreach (var module in this.AvailableModules) {
				module.IsEditing = isEditing;
			}
		}

		public void SaveFavorites() {
			// Populate favorite modules list
			var favoriteModules = new FavoriteModules();
			foreach (var favorited in this.AvailableModules.Where(m => m.IsFavorite)) {
				favoriteModules.Modules.Add(new FavoriteModule() {ID = favorited.Module.ID});
			}

			// Save list
			this.isolatedStorageServiceFacade.SaveFavoriteModules(favoriteModules);

			// Refresh list
			this.LoadFavoriteModules();
		}

		public void ChangeClient() {
			this.navigationServiceFacade.Navigate(
				new Uri("/Pages/ChangeClient/CreateClientView.xaml", UriKind.Relative));
		}

		public void AdvancedSettings(int landOnPageIndex = 0) {
			this.navigationServiceFacade.Navigate(
				new Uri(
					"/Pages/AdvancedSettings/AdvancedSettingsView.xaml?Index=" +
					landOnPageIndex, UriKind.Relative));
		}

		#region Properties

		public ObservableCollection<ModuleModel> FavoriteModules { get; private set; }
		public ObservableCollection<ModuleModel> AvailableModules { get; private set; }

		public bool IsEditingFavorites {
			get { return this.isEditingFavorites; }
			set {
				this.isEditingFavorites = value;
				RaisePropertyChanged("IsEditingFavorites");
			}
		}

		public int TileRows {
			get { return this.tileRows; }
		}

		#endregion

		public void NavigateTo(Module module) {
			// Navigate to module view
			if (!this.navigationServiceFacade.Navigate(
				new Uri(module.View, UriKind.Relative)))
				this.log.Warn("Couldn't navigate to \"{0}\" module's view", module.Name);
		}
	}
}
