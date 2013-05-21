using System.Linq;
using System.Text;
using EveWindowsPhone.Modules;

namespace EveWindowsPhone.Adapters {
	public interface IIsolatedStorageServiceFacade {
		FavoriteModules GetFavoriteModules();
		void SaveFavoriteModules(FavoriteModules modules);

		bool ContainsSetting(string settingKey);
		T GetSetting<T>(string settingKey);
		void SetSetting<T>(T value, string settingKey);
		void SetDefault<T>(T defaultValue, string key);
	}
}
