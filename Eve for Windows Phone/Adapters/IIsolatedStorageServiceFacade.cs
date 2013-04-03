using System.Linq;
using System.Text;
using EveWindowsPhone.Modules;

namespace EveWindowsPhone.Adapters {
	public interface IIsolatedStorageServiceFacade {
		FavoriteModules GetFavoriteModules();
		void SaveFavoriteModules(FavoriteModules modules);
	}
}
