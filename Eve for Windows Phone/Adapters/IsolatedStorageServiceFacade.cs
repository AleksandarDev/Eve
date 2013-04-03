using System;
using System.IO.IsolatedStorage;
using EveWindowsPhone.Modules;
using Newtonsoft.Json;

namespace EveWindowsPhone.Adapters {
	public class IsolatedStorageServiceFacade : IIsolatedStorageServiceFacade {
		private const string FavoriteModulesKey = "FavoriteModules";


		public FavoriteModules GetFavoriteModules() {
			if (!IsolatedStorageSettings.ApplicationSettings.Contains(FavoriteModulesKey))
				throw new InvalidOperationException("FavoriteModules is missing from Isolated Storage");

			try {
				return
					JsonConvert.DeserializeObject<FavoriteModules>(
						IsolatedStorageSettings.ApplicationSettings[FavoriteModulesKey] as String);
			}
			catch (Exception) {
				System.Diagnostics.Debug.WriteLine("Invalid favorites data!");
				return null;
			}
		}

		public void SaveFavoriteModules(FavoriteModules modules) {
			if (modules == null) throw new ArgumentNullException("modules");

			try {
				IsolatedStorageSettings.ApplicationSettings[FavoriteModulesKey] = JsonConvert.SerializeObject(modules);
				IsolatedStorageSettings.ApplicationSettings.Save();
			}
			catch (Exception) {
				System.Diagnostics.Debug.WriteLine("Can't save favorites!");
			}
		}
	}
}