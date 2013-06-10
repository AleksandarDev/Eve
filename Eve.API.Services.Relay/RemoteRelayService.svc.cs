﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web.UI.WebControls;
using Eve.API.Services.Common;
using Eve.API.Services.Common.Modules.Ambiental;
using Eve.API.Services.Common.Modules.Lights;
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
		// TODO Check if signed in

		/// <summary>
		/// Retrieve callback contract of subscribed client
		/// </summary>
		/// <param name="details">Details about client to get callback from</param>
		/// <returns>Returns null if requested client isn't subscribed or invalid</returns>
		/// <remarks>This also checks if user and client data is valid, returns null if invalid</remarks>
		protected IEveAPIService GetCallback(ServiceRequestDetails details) {
			if (details == null) {

				return null;
			}

			var client = RelayManager.GetClient(details.Client.ID);
			if (client == null) return null;

			return client.Callback as IEveAPIService;
		}

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

		#region Touch implementation

		public bool SendTrackPadMessage(ServiceRequestDetails details,
										TrackPadMessage message) {
			var callback = this.GetCallback(details);
			if (callback == null) return false;

			return callback.SendTrackPadMessage(details, message);
		}

		public bool SendButtonMessage(ServiceRequestDetails details,
									  ButtonMessage message) {
			var callback = this.GetCallback(details);
			if (callback == null) return false;

			return callback.SendButtonMessage(details, message);
		}

		#endregion

		#region DisplayEnhancements implementation

		public bool SetZoom(ServiceRequestDetails details, int zoomValue) {
			var callback = this.GetCallback(details);
			if (callback == null) return false;

			return callback.SetZoom(details, zoomValue);
		}

		#endregion

		#region Lights implementation

		public Light[] GetLights(ServiceRequestDetails details) {
			var callback = this.GetCallback(details);
			if (callback == null) return null;

			return callback.GetLights(details);
		}

		public bool SetLightState(ServiceRequestDetails details, int id, bool state) {
			var callback = this.GetCallback(details);
			if (callback == null) return false;

			return callback.SetLightState(details, id, state);
		}

		#endregion

		#region Ambiental implementation

		public AmbientalLight[] GetAmbientalLights(ServiceRequestDetails details) {
			var callback = this.GetCallback(details);
			if (callback == null) return null;

			return callback.GetAmbientalLights(details);
		}

		public bool SetAmbientalLightState(ServiceRequestDetails details, int id, bool state) {
			var callback = this.GetCallback(details);
			if (callback == null) return false;

			return callback.SetAmbientalLightState(details, id, state);
		}

		public bool SetAmbientalLightColor(ServiceRequestDetails details, int id, byte r, byte g, byte b, byte a) {
			var callback = this.GetCallback(details);
			if (callback == null) return false;

			return callback.SetAmbientalLightColor(details, id, r, g, b, a);
		}

		#endregion
	}
}
