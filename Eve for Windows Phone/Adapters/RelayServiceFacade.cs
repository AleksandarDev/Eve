using EveWindowsPhone.Communication;

namespace EveWindowsPhone.Adapters {
	public class RelayServiceFacade : IRelayServiceFacade {
		public RelayProxy Proxy { get; private set; }


		public RelayServiceFacade() {
			this.Proxy = new RelayProxy();
			this.Proxy.Open();
		}
	}
}