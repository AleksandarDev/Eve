using System;
using System.Collections;
using System.Collections.Generic;
using EveWindowsPhone.Adapters;
using EveWindowsPhone.Modules;
using EveWindowsPhone.ViewModels;

namespace EveWindowsPhone.Pages.AdvancedSettings {
	public class AdvancedSettingsViewModel : NotificationObject {
		private readonly INavigationServiceFacade navigationServiceFacade;
		private readonly IIsolatedStorageServiceFacade isolatedStorageServiceFacade;

		private string clientID;
		private int favoriteRows;
		private bool activateZoomOnTouch;
		private int activateZoomOnTouchValue;
		private bool activateZoomOnKeyboard;
		private int activateZoomOnKeyboardValue;
		private int lightsRefreshRateValue;
		private int ambientalRefreshRateValue;

		public AdvancedSettingsViewModel(
			INavigationServiceFacade navigationServiceFacade,
			IIsolatedStorageServiceFacade isolatedStorageServiceFacade) {
			this.navigationServiceFacade = navigationServiceFacade;
			this.isolatedStorageServiceFacade = isolatedStorageServiceFacade;

			// Generate zoom values
			this.ActivateZoomValuesList = new List<int>() {
				200,
				400,
				600,
				1000,
				1600
			};

			// Generate row values
			this.FavoriteRowsList = new List<int>() {
				2,
				3,
				4
			};

			// Generate refresh rates
			this.RefreshRatesList = new List<int>() {
				200,
				500,
				1000,
				5000
			};

			this.LoadSettings();
		}

		public void LoadSettings() {
			// CLientID settings
			this.ClientID =
				this.isolatedStorageServiceFacade.GetSetting<string>(
					IsolatedStorageServiceFacade.ClientIDKey);

			// Favorites settings
			this.FavoriteRows =
				this.isolatedStorageServiceFacade.GetSetting<int>(
					IsolatedStorageServiceFacade.FavoriteRowsKey);

			// Zoom settings
			this.ActivateZoomOnTouch =
				this.isolatedStorageServiceFacade.GetSetting<bool>(
					IsolatedStorageServiceFacade.ActivateZoomOnTouchKey);

			this.ActivateZoomOnTouchValue =
				this.isolatedStorageServiceFacade.GetSetting<int>(
					IsolatedStorageServiceFacade.ActivateZoomOnTouchValueKey);

			// Keyboard settings
			this.ActivateZoomOnKeyboard =
				this.isolatedStorageServiceFacade.GetSetting<bool>(
					IsolatedStorageServiceFacade.ActivateZoomOnKeyboardKey);

			this.activateZoomOnKeyboardValue =
				this.isolatedStorageServiceFacade.GetSetting<int>(
					IsolatedStorageServiceFacade.ActivateZoomOnKeyboardValueKey);

			// Lights settings
			this.LightsRefreshRateValue =
				this.isolatedStorageServiceFacade.GetSetting<int>(
					IsolatedStorageServiceFacade.LightsRefreshRateKey);

			// Ambiental settings
			this.AmbientalsRefreshRateValue =
				this.isolatedStorageServiceFacade.GetSetting<int>(
					IsolatedStorageServiceFacade.AmbientalRefreshRateKey);
		}

		public void ClearFavoriteModules() {
			// Saves empty list of favorites (clears facorites)
			this.isolatedStorageServiceFacade.SaveFavoriteModules(new FavoriteModules());
		}

		#region Properties

		public string ClientID {
			get { return this.clientID; }
			set { this.clientID = value;
				RaisePropertyChanged("ClientID");
				this.isolatedStorageServiceFacade.SetSetting(
					this.clientID,
					IsolatedStorageServiceFacade.ClientIDKey);
			}
		}

		public int FavoriteRows {
			get { return this.favoriteRows; }
			set {
				this.favoriteRows = value;
				RaisePropertyChanged("FavoriteRows");
				this.isolatedStorageServiceFacade.SetSetting(
					this.favoriteRows,
					IsolatedStorageServiceFacade.FavoriteRowsKey);
			}
		}

		public bool ActivateZoomOnTouch {
			get { return this.activateZoomOnTouch; }
			set {
				this.activateZoomOnTouch = value;
				RaisePropertyChanged("ActivateZoomOnTouch");
				this.isolatedStorageServiceFacade.SetSetting(
					this.activateZoomOnTouch,
					IsolatedStorageServiceFacade.ActivateZoomOnTouchKey);
			}
		}

		public int ActivateZoomOnTouchValue {
			get { return this.activateZoomOnTouchValue; }
			set {
				this.activateZoomOnTouchValue = value;
				RaisePropertyChanged("ActivateZoomOnTouchValue");
				this.isolatedStorageServiceFacade.SetSetting(
					this.activateZoomOnTouchValue,
					IsolatedStorageServiceFacade.ActivateZoomOnTouchValueKey);
			}
		}

		public bool ActivateZoomOnKeyboard {
			get { return this.activateZoomOnKeyboard; }
			set {
				this.activateZoomOnKeyboard = value;
				RaisePropertyChanged("ActivateZoomOnKeyboard");
				this.isolatedStorageServiceFacade.SetSetting(
					this.activateZoomOnKeyboard,
					IsolatedStorageServiceFacade.ActivateZoomOnKeyboardKey);
			}
		}

		public int ActivateZoomOnKeyboardValue {
			get { return this.activateZoomOnKeyboardValue; }
			set {
				this.activateZoomOnKeyboardValue = value;
				RaisePropertyChanged("ActivateZoomOnKeyboardValue");
				this.isolatedStorageServiceFacade.SetSetting(
					this.activateZoomOnKeyboardValue,
					IsolatedStorageServiceFacade.ActivateZoomOnKeyboardValueKey);
			}
		}

		public List<int> ActivateZoomValuesList { get; protected set; }
		public List<int> FavoriteRowsList { get; protected set; }

		public List<int> RefreshRatesList { get; protected set; }

		public int LightsRefreshRateValue {
			get { return this.lightsRefreshRateValue; }
			set { this.lightsRefreshRateValue = value;
				RaisePropertyChanged("LightsRefreshRateValue");
				this.isolatedStorageServiceFacade.SetSetting(
					this.lightsRefreshRateValue,
					IsolatedStorageServiceFacade.LightsRefreshRateKey);
			}
		}

		public int AmbientalsRefreshRateValue {
			get { return this.ambientalRefreshRateValue; }
			set {
				this.ambientalRefreshRateValue = value;
				RaisePropertyChanged("AmbientalsRefreshRateValue");
				this.isolatedStorageServiceFacade.SetSetting(
					this.ambientalRefreshRateValue,
					IsolatedStorageServiceFacade.AmbientalRefreshRateKey);
			}
		}

		#endregion
	}
}