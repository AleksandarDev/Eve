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
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class RelayService : IRelayService {
		public RelayService() {}


		#region IRelayService

		public void Subscribe() {
			RelayManager.RegisterClient(new ServiceClient("SampleClient", "SampleClient") {
				Callback =
					OperationContext.Current.GetCallbackChannel<IRelayCallbackContract>()
			});
		}

		public void Unsibscribe() {
			throw new NotImplementedException();
		}

		public void UpdateClientState(ClientState state) {
			throw new NotImplementedException();
		}

		#endregion
	}
}