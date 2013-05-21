using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveWindowsPhone.Communication;

namespace EveWindowsPhone.Adapters {
	public interface IRelayServiceFacade {
		RelayProxy Proxy { get; }
	}
}
