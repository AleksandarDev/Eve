using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
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
		/// <param name="client">Client ID for which to get callback</param>
		/// <returns>Returns null if requested client isn't subscribed or invalid</returns>
		/// <remarks>This also checks if user and client data is valid, returns null if invalid</remarks>
		protected IEveAPIService GetCallback(string client) {
			if (String.IsNullOrEmpty(client))
				return null;

			var clientObject = RelayManager.GetClient(client);
			if (clientObject == null) return null;

			return clientObject.Callback as IEveAPIService;
		}

		#region IEveAPIService implementation

		[WebInvoke(
			BodyStyle = WebMessageBodyStyle.Wrapped,
			Method = "POST",
			UriTemplate = "user/signin")]
		public bool SignIn(ServiceUser user) {
			if (user.UserName == "admin" &&
				user.PasswordHash == "1234")
				return true;
			return false;
		}

		[WebInvoke(
			BodyStyle = WebMessageBodyStyle.Wrapped,
			Method = "POST",
			UriTemplate = "user/signout")]
		public bool SignOut(ServiceUser user) {
			throw new NotImplementedException();
		}

		[WebGet(UriTemplate = "clients/")]
		public ServiceClient[] GetAvailableClients() {
			return RelayManager.GetClients().ToArray();
		}

		#endregion

		#region Touch implementation

		[WebInvoke(
			BodyStyle = WebMessageBodyStyle.Wrapped,
			Method = "POST",
			UriTemplate = "modules/touch/trackpad")]
		public bool SendTrackPadMessage(string client,
										TrackPadMessage message) {
			var callback = this.GetCallback(client);
			if (callback == null) return false;

			return callback.SendTrackPadMessage(client, message);
		}

		[WebInvoke(
			BodyStyle = WebMessageBodyStyle.Wrapped,
			Method = "POST",
			UriTemplate = "modules/touch/button")]
		public bool SendButtonMessage(string client,
									  ButtonMessage message) {
			var callback = this.GetCallback(client);
			if (callback == null) return false;

			return callback.SendButtonMessage(client, message);
		}

		#endregion

		#region DisplayEnhancements implementation

		[WebInvoke(
			BodyStyle = WebMessageBodyStyle.Wrapped,
			Method = "POST",
			UriTemplate = "modules/displayenhancements")]
		public bool SetZoom(string client, int zoomValue) {
			var callback = this.GetCallback(client);
			if (callback == null) return false;

			return callback.SetZoom(client, zoomValue);
		}

		#endregion

		#region Lights implementation

		[WebInvoke(
			BodyStyle = WebMessageBodyStyle.Wrapped,
			Method = "POST",
			UriTemplate = "modules/lights/all")]
		public Light[] GetLights(string client) {
			var callback = this.GetCallback(client);
			if (callback == null) return null;

			return callback.GetLights(client);
		}

		[WebInvoke(
			BodyStyle = WebMessageBodyStyle.Wrapped,
			Method = "POST",
			UriTemplate = "modules/lights")]
		public bool SetLightState(string client, int id, bool state) {
			var callback = this.GetCallback(client);
			if (callback == null) return false;

			return callback.SetLightState(client, id, state);
		}

		#endregion

		#region Ambiental implementation

		[WebInvoke(
			BodyStyle = WebMessageBodyStyle.Wrapped,
			Method = "POST",
			UriTemplate = "modules/ambiental/all")]
		public AmbientalLight[] GetAmbientalLights(string client) {
			var callback = this.GetCallback(client);
			if (callback == null) return null;

			return callback.GetAmbientalLights(client);
		}

		[WebInvoke(
			BodyStyle = WebMessageBodyStyle.Wrapped,
			Method = "POST",
			UriTemplate = "modules/ambiental/state")]
		public bool SetAmbientalLightState(string client, int id, bool state) {
			var callback = this.GetCallback(client);
			if (callback == null) return false;

			return callback.SetAmbientalLightState(client, id, state);
		}

		[WebInvoke(
			BodyStyle = WebMessageBodyStyle.Wrapped,
			Method = "POST",
			UriTemplate = "modules/ambiental/color")]
		public bool SetAmbientalLightColor(string client, int id, byte r, byte g, byte b, byte a) {
			var callback = this.GetCallback(client);
			if (callback == null) return false;

			return callback.SetAmbientalLightColor(client, id, r, g, b, a);
		}

		#endregion
	}
}
