﻿using System;
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

		public bool UpdateClientState(ClientState state) {
			throw new NotImplementedException();
		}

		#endregion

		#region IEveAPIService

		public bool SendTrackPadMessage(ServiceRequestDetails details,
										TrackPadMessage message) {
			throw new NotImplementedException();
		}

		public bool SendButtonMessage(ServiceRequestDetails details,
									  ButtonMessage message) {
			throw new NotImplementedException();
		}

		public string Ping(string yourName) {
			return "Hello " + yourName;
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
	}
}