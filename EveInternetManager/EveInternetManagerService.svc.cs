using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace EveInternetManager {
	[ServiceBehavior(
		InstanceContextMode = InstanceContextMode.Single,
		ConcurrencyMode = ConcurrencyMode.Reentrant)]
	public class EveInternetManagerService : IEveInternetManagerService {
		private Dictionary<string, HostDescription> hosts;
 

		public EveInternetManagerService() {
			System.Diagnostics.Debug.WriteLine(String.Format("EveInternetManager Service created [{0}]", this.GetHashCode()));

			this.hosts = new Dictionary<string, HostDescription>();
		}

		~EveInternetManagerService() {
			System.Diagnostics.Debug.WriteLine(String.Format("EveInternetManager Service destroyed [{0}]", this.GetHashCode()));
		}


		public bool RegisterHost(HostDescription description) {
			if (description == null) return false;

			if (this.hosts.ContainsKey(description.ID))
				return false;

			this.hosts.Add(description.ID, description);
			System.Diagnostics.Debug.WriteLine(String.Format("Adding host \"{0}\"({1}) at address [{2}]",
			                                                 description.Alias, description.ID, description.Address));
			return true;
		}

		public HostDescription GetHostInfo(string id) {
			if (!this.hosts.ContainsKey(id)) return null;
			return this.hosts[id];
		}
	}
}
