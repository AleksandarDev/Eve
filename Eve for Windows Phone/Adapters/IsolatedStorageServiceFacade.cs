using System;
using System.IO.IsolatedStorage;
using Eve.Diagnostics.Logging;
using EveWindowsPhone.Modules;
using Newtonsoft.Json;

namespace EveWindowsPhone.Adapters {
	public class IsolatedStorageServiceFacade : IIsolatedStorageServiceFacade {
		public const string ActivateZoomOnTouchKey = "ActivateZoomOnTouch";
		public const string ActivateZoomOnTouchValueKey = "ActivateZoomOnTouchValue";
		public const string ActivateZoomOnKeyboardKey = "ActivateZoomOnKeyboard";
		public const string ActivateZoomOnKeyboardValueKey = "ActivateZoomOnKeyboardValue";
		public const string FavoriteModulesKey = "FavoriteModules";
		public const string FavoriteRowsKey = "FavoriteRowsNumber";
		public const string ClientIDKey = "ClientUID";
		public const string LightsRefreshRateKey = "LightsRefreshRate";
		public const string AmbientalRefreshRateKey = "AmbientalRefreshRate";

		protected readonly Log.LogInstance log =
			new Log.LogInstance(typeof(IsolatedStorageServiceFacade));


		public FavoriteModules GetFavoriteModules() {
			return this.GetData<FavoriteModules>(FavoriteModulesKey);
		}

		public void SaveFavoriteModules(FavoriteModules modules) {
			if (modules == null) throw new ArgumentNullException("modules");

			this.SaveData<FavoriteModules>(modules, FavoriteModulesKey);
		}

		public bool ContainsSetting(string settingKey) {
			return this.ContainsData(settingKey);
		}

		public T GetSetting<T>(string settingKey) {
			return this.GetData<T>(settingKey);
		}

		public void SetSetting<T>(T value, string settingKey) {
			this.SaveData<T>(value, settingKey);

			this.log.Info("Setting [{0}] saved", settingKey);
		}

		private bool ContainsData(string key) {
			return IsolatedStorageSettings.ApplicationSettings.Contains(key);
		}

		private void SaveData<T>(T data, string key) {
			IsolatedStorageSettings.ApplicationSettings[key] = JsonConvert.SerializeObject(data);

			this.log.Info("Data [{0}] saved", key);
		}

		private T GetData<T>(string key) {
			if (this.ContainsData(key))
				return JsonConvert.DeserializeObject<T>(IsolatedStorageSettings.ApplicationSettings[key] as String);
			throw new InvalidOperationException("Requested data key not found");
		}

		public void SetDefault<T>(T defaultValue, string key) {
			if (!this.ContainsSetting(key)) {
				this.SetSetting(defaultValue, key);
				this.log.Info("Default [{0}] saved", key);
			}
		}
	}
}