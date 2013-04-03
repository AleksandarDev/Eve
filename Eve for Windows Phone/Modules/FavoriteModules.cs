using System.Collections.Generic;

namespace EveWindowsPhone.Modules {
	public class FavoriteModules {
		public List<FavoriteModule> Modules { get; private set; } 


		public FavoriteModules() {
			this.Modules = new List<FavoriteModule>();
		}
	}
}