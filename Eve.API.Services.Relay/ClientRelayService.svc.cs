using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Eve.API.Services.Common;
using Eve.API.Services.Contracts;

namespace Eve.API.Services.Relay {
	[AspNetCompatibilityRequirements(
		RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	[ServiceBehavior(
		InstanceContextMode = InstanceContextMode.Single,
		ConcurrencyMode = ConcurrencyMode.Reentrant, 
		UseSynchronizationContext = false)]
	public class ClientRelayService : IClientRelayService {
		#region IClientRelayService implementation

		public bool Subscribe(ServiceClient clientData) {
			clientData.Callback =
				OperationContext.Current.GetCallbackChannel<IEveAPIService>();
			return RelayManager.RegisterClient(clientData);
		}

		public bool Unsibscribe(ServiceClient clientData) {
			return RelayManager.UnregisterClient(clientData.ID);
		}

		public string ClientPing(string yourName) {
			return "Hello " + yourName;
		}

		#endregion
	}
}