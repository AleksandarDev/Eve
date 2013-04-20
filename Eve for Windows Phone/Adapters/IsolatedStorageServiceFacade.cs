using System;
using System.IO.IsolatedStorage;
using EveWindowsPhone.Modules;
using Newtonsoft.Json;

namespace EveWindowsPhone.Adapters {
	public class IsolatedStorageServiceFacade : IIsolatedStorageServiceFacade {
		public const string FavoriteModulesKey = "FavoriteModules";
		public const string FavoriteRowsKey = "FavoriteRowsNumber";


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
		}

		private bool ContainsData(string key) {
			return IsolatedStorageSettings.ApplicationSettings.Contains(key);
		}

		private void SaveData<T>(T data, string key) {
			IsolatedStorageSettings.ApplicationSettings[key] = JsonConvert.SerializeObject(data);
		}

		private T GetData<T>(string key) {
			if (this.ContainsData(key))
				return JsonConvert.DeserializeObject<T>(IsolatedStorageSettings.ApplicationSettings[key] as String);
			throw new InvalidOperationException("Requested data key not found");
		}
	}
}