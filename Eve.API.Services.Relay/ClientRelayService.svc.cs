﻿using System.ServiceModel;
using System.ServiceModel.Activation;
using Eve.API.Services.Common;
using Eve.API.Services.Contracts;

namespace Eve.API.Services.Relay {
	[AspNetCompatibilityRequirements(
		RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	[ServiceBehavior(
		InstanceContextMode = InstanceContextMode.PerCall,
		ConcurrencyMode = ConcurrencyMode.Reentrant, 
		UseSynchronizationContext = false)]
	public class ClientRelayService : IClientRelayService {
		#region IClientRelayService implementation

		public bool Subscribe(ServiceClient clientData) {
			// TODO Unsubscribe client after some amount of time no ping
			clientData.Callback =
				OperationContext.Current.GetCallbackChannel<IEveAPIService>();
			return RelayManager.RegisterClient(clientData);
		}

		// TODO rename to Unsubscribe
		public bool Unsibscribe(ServiceClient clientData) {
			return RelayManager.UnregisterClient(clientData.ID);
		}

		public string ClientPing(string yourName) {
			return "Hello " + yourName;
		}

		#endregion
	}
}