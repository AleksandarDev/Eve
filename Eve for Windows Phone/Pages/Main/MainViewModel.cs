using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Eve.Diagnostics.Logging;
using EveWindowsPhone.Adapters;
using EveWindowsPhone.Modules;
using EveWindowsPhone.Pages.AdvancedSettings;
using EveWindowsPhone.ViewModels;

namespace EveWindowsPhone.Pages.Main {
	public class MainViewModel : NotificationObject {
		private readonly Log.LogInstance log =
			new Log.LogInstance(typeof(MainViewModel));

		private readonly INavigationServiceFacade navigationServiceFacade;
		private readonly IIsolatedStorageServiceFacade isolatedStorageServiceFacade;
		private readonly IRelayServiceFacade relayServiceFacade;

		private bool isEditingFavorites;
		private int tileRows;
		private string clientID;
		private bool isConnected;


		public MainViewModel(INavigationServiceFacade navigationServiceFacade,
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

			this.FavoriteModules = new ObservableCollection<ModuleModel>();
			this.AvailableModules = new ObservableCollection<ModuleModel>();

			this.LoadSettings();
			this.LoadModules();

			// Set connection info
			this.IsConnected = this.relayServiceFacade.Proxy.IsConnected;
			this.relayServiceFacade.Proxy.OnOpened += proxy => this.IsConnecting = false;
			this.relayServiceFacade.Proxy.OnClosed += proxy => this.IsConnecting = true;
			this.timer = new DispatcherTimer() {Interval = TimeSpan.FromSeconds(10)};
			this.timer.Tick += (sender, args) => {
				this.IsConnecting = !this.relayServiceFacade.Proxy.IsConnected;

				// Show error message if got disconnected
				if (!this.IsConnected) {
					MessageBox.Show(
						"Couldn't connect to server. Check your internet connection...", "SyncUp",
						MessageBoxButton.OK);
					this.timer.Stop();
				}
			};
			this.timer.Start();

			this.log.Info("View model created");
		}

		private DispatcherTimer timer;

		public void LoadSettings() {
			this.tileRows = this.isolatedStorageServiceFacade.GetSetting<int>(
				IsolatedStorageServiceFacade.FavoriteRowsKey);

			this.ClientID = this.isolatedStorageServiceFacade.GetSetting<string>(
				IsolatedStorageServiceFacade.ClientIDKey);
		}

		public void LoadModules() {
			// Get modules locator object
			var modulesLocator =
				Application.Current.Resources["ModulesLocator"] as ModulesLocator;
			if (modulesLocator == null)
				throw new NullReferenceException("Can't find ModulesLocator");

			this.AvailableModules = modulesLocator.AvailableModules;
			this.OwnedModules = modulesLocator.OwnedModules;

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
<<<<<<< HEAD
					var available = this.AvailableModules.First(m => m.ModuleAttribute.ID == module.ID);
=======
					//var available = this.AvailableModules.First(m => m.ModuleAttribute.ID == module.ID);
					var available = this.OwnedModules.First(m => m.ModuleAttribute.ID == module.ID);
>>>>>>> master
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
			//foreach (var module in this.AvailableModules) {
			foreach (var module in this.OwnedModules) {
				module.IsEditing = isEditing;
			}
		}

		public void SaveFavorites() {
			// Populate favorite modules list
			var favoriteModules = new FavoriteModules();
<<<<<<< HEAD
			foreach (var favorited in this.AvailableModules.Where(m => m.IsFavorite)) {
=======
			//foreach (var favorited in this.AvailableModules.Where(m => m.IsFavorite)) {
			foreach (var favorited in this.OwnedModules.Where(m => m.IsFavorite)) {
>>>>>>> master
				favoriteModules.Modules.Add(new FavoriteModule() {ID = favorited.ModuleAttribute.ID});
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

		public void AdvancedSettings() {
			AdvancedSettingsView.NavigateWithIndex(this.navigationServiceFacade, 0);
		}

		public void AdvancedSettingsFavorites() {
			AdvancedSettingsView.NavigateWithIndex(this.navigationServiceFacade, 1);
		}

		public void NavigateTo(ModuleAttribute moduleAttribute) {
			// Navigate to module view
			if (!this.navigationServiceFacade.Navigate(
				new Uri(moduleAttribute.View, UriKind.Relative)))
				this.log.Warn("Couldn't navigate to \"{0}\" module's view", moduleAttribute.Name);
		}

		#region Properties

		// TODO FavoriteModules collection is redundant, remove it
		public ObservableCollection<ModuleModel> FavoriteModules { get; private set; }
		public ObservableCollection<ModuleModel> AvailableModules { get; private set; }
		public ObservableCollection<ModuleModel> OwnedModules { get; private set; } 

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

		public string ClientID {
			get { return this.clientID; }
			set {
				this.clientID = value;
				this.RaisePropertyChanged(() => this.ClientID);
				this.isolatedStorageServiceFacade.SetSetting(
					this.clientID,
					IsolatedStorageServiceFacade.ClientIDKey);
			}
		}

		public bool IsConnected {
			get { return this.isConnected; }
			set {
				this.isConnected = value;
				this.RaisePropertyChanged(() => this.IsConnected);
			}
		}

		public bool IsConnecting {
			get { return !this.isConnected; }
			set {
				this.IsConnected = !value;
				this.RaisePropertyChanged(() => this.IsConnecting);
			}
		}

		#endregion
	}
}
