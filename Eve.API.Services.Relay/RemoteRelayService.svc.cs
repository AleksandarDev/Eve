using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Eve.API.Services.Contracts;
using Eve.API.Services.Contracts.Services;
using Eve.API.Services.Contracts.Services.Interfaces;

namespace Eve.API.Services.Relay {
	[AspNetCompatibilityRequirements(
		RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	[ServiceBehavior(
		InstanceContextMode = InstanceContextMode.Single,
		ConcurrencyMode = ConcurrencyMode.Reentrant,
		UseSynchronizationContext = false)]
	public class RemoteRelayService : IEveAPIService {
		public RemoteRelayService() {}


		#region IEveAPIService implementation

		public string Ping(string yourName) {
			if (RelayManager.GetClients().Any())
				return
					RelayManager.GetClients().First().Callback.PingRequest("relay " + yourName);
			else return "No client available";
		}

		public bool SignIn(ServiceUser user) {
			throw new NotImplementedException();
		}

		public bool SignOut(ServiceUser user) {
			throw new NotImplementedException();
		}

		public ClientState GetClientState() {
			throw new NotImplementedException();
		}

		public ServiceClient[] GetAvailableClients() {
			return RelayManager.GetClients().ToArray();
		}

		#endregion

		#region ITouchService implementation

		public bool SendTrackPadMessage(ServiceRequestDetails details,
								TrackPadMessage message) {
			throw new NotImplementedException();
		}

		public bool SendButtonMessage(ServiceRequestDetails details,
									  ButtonMessage message) {
			throw new NotImplementedException();
		}

		#endregion
	}
}