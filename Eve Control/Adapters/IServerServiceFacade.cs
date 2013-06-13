using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EveControl.Communication;

namespace EveControl.Adapters {
	public interface IServerServiceFacade {
		// Common
		EveControl.RelayServiceReference.ServiceClient Client { get; }

		// Server service
		// TODO Add server proxy

		// Relay service
		RelayProxy RelayProxy { get; }
		Task<bool> OpenRelayConnectionAsync();
		Task<bool> CloseRelayConnectionAsync();
	}
}
