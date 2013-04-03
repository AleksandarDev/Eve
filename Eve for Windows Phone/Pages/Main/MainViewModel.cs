using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using EveWindowsPhone.Adapters;
using EveWindowsPhone.Modules;
using EveWindowsPhone.ViewModels;

namespace EveWindowsPhone.Pages.Main {
	public class MainViewModel : NotificationObject {
		private readonly INavigationServiceFacade navigationServiceFacade;
		private readonly IIsolatedStorageServiceFacade isolatedStorageServiceFacade;


		public MainViewModel(INavigationServiceFacade navigationServiceFacade, IIsolatedStorageServiceFacade isolatedStorageServiceFacade) {
			if (navigationServiceFacade == null) throw new ArgumentNullException("navigationServiceFacade");
			this.navigationServiceFacade = navigationServiceFacade;
			this.isolatedStorageServiceFacade = isolatedStorageServiceFacade;

			this.FavoriteModules = new ObservableCollection<ModuleModel>();
			this.AvailableModules = new ObservableCollection<ModuleModel>();

			this.LoadModules();
		}


		public void LoadModules() {
			// Get modules locator object
			var modulesLocator = Application.Current.Resources["ModulesLocator"] as ModulesLocator;
			if (modulesLocator == null) throw new NullReferenceException("Can't find ModulesLocator");

			this.AvailableModules = modulesLocator.AvailableModules;

			this.LoadFavoriteModules();
		}

		private void LoadFavoriteModules() {
			this.FavoriteModules.Clear();

			try {
				var savedFavorites = this.isolatedStorageServiceFacade.GetFavoriteModules();
				System.Diagnostics.Debug.WriteLine("{0} saved favorites retrieved", new object[] {savedFavorites.Modules.Count});
				foreach (var module in savedFavorites.Modules) {
					var available = this.AvailableModules.First(m => m.Module.ID == module.ID);
					available.IsFavorite = true;
					this.FavoriteModules.Add(available);
				}
			}
			catch (InvalidOperationException) {
				System.Diagnostics.Debug.WriteLine("No favorite modules saved");
			}
		}

		public void SetModulesIsEditing(bool mode) {
			foreach (var module in this.AvailableModules) {
				module.IsEditing = mode;
			}
		}

		public void SaveFavorites() {
			// Populate favorite modules list
			var favoriteModules = new FavoriteModules();
			foreach (var favorited in this.AvailableModules.Where(m => m.IsFavorite)) {
				favoriteModules.Modules.Add(new FavoriteModule() { ID = favorited.Module.ID });
			}

			// Save list
			this.isolatedStorageServiceFacade.SaveFavoriteModules(favoriteModules);

			// Refresh list
			this.LoadFavoriteModules();
		}

		#region Properties

		public ObservableCollection<ModuleModel> FavoriteModules { get; private set; } 
		public ObservableCollection<ModuleModel> AvailableModules { get; private set; }

		private bool isEditingFavorites;
		public bool IsEditingFavorites {
			get { return this.isEditingFavorites; }
			set {
				this.isEditingFavorites = value;
				RaisePropertyChanged("IsEditingFavorites");
			}
		}

		#endregion

		public void NavigateTo(Module module) {
			// Navigate to module view
			if (!this.navigationServiceFacade.Navigate(new Uri(module.View, UriKind.Relative)))
				System.Diagnostics.Debug.WriteLine(String.Format("Couldn't navigate to \"{0}\" module's view", module.Name));
		}
	}
}
