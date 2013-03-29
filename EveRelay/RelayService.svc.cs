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

namespace EveRelay {
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
	public class RelayService : IRelayService {
		public bool SendTrackPadMessage(ServiceRequestDetails details, TrackPadMessage message) {
			throw new NotImplementedException();
		}

		public bool SendButtonMessage(ServiceRequestDetails details, ButtonMessage message) {
			throw new NotImplementedException();
		}

		public bool SignIn(ServiceUser user) {
			throw new NotImplementedException();
		}

		public bool SignOut(ServiceUser user) {
			throw new NotImplementedException();
		}

		public string Ping(string yourName) {
			return "Hello" + yourName;
		}

		public void PingOneWay(string yourName) {
			try {
				var callback = OperationContext.Current.GetCallbackChannel<IRelayCallbackContract>();
				callback.PingOneWayResponse("Hello " + yourName);
			}
			catch (Exception) {
				System.Diagnostics.Debug.WriteLine("Error occured in Ping method");
			}
		}

		public void Subscribe() {
			throw new NotImplementedException();
		}

		public void Unsibscribe() {
			throw new NotImplementedException();
		}

		public ServiceClient[] GetAvailableClients() {
			throw new NotImplementedException();
		}

		public void GetClientState() {
			throw new NotImplementedException();
		}

		public bool UpdateClientState() {
			throw new NotImplementedException();
		}
	}
}
