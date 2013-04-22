using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Eve.API.Services.Common;
using Eve.API.Services.Common.Modules.Touch;
using Eve.API.Services.Contracts;

namespace Eve.API.Services.Relay {
	[AspNetCompatibilityRequirements(
		RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	[ServiceBehavior(
		InstanceContextMode = InstanceContextMode.Single,
		ConcurrencyMode = ConcurrencyMode.Reentrant,
		UseSynchronizationContext = false)]
	public class RemoteRelayService : IEveAPIService {
		#region IEveAPIService implementation

		public bool SignIn(ServiceUser user) {
			throw new NotImplementedException();
		}

		public bool SignOut(ServiceUser user) {
			throw new NotImplementedException();
		}

		public ServiceClient[] GetAvailableClients() {
			return RelayManager.GetClients().ToArray();
		}

		#endregion

		#region ITouchService implementation

		public bool SendTrackPadMessage(ServiceRequestDetails details,
										TrackPadMessage message) {
			var client = RelayManager.GetClient(details.Client.ID);
			if (client == null) return false;

			var callback = client.Callback as IEveAPIService;
			if (callback == null) return false;

			return callback.SendTrackPadMessage(details, message);
		}

		public bool SendButtonMessage(ServiceRequestDetails details,
									  ButtonMessage message) {
			var client = RelayManager.GetClient(details.Client.ID);
			if (client == null) return false;

			var callback = client.Callback as IEveAPIService;
			if (callback == null) return false;

			return callback.SendButtonMessage(details, message);
		}

		#endregion
	}
}
