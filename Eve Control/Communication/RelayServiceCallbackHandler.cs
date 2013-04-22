using Eve.API.Services.Common;
using Eve.API.Services.Common.Modules.Touch;
using Eve.API.Touch;
using EveControl.RelayServiceReference;

namespace EveControl.Communication {
	/// <summary>
	/// Implementation of Relay Service Callback methods
	/// </summary>
	public class RelayServiceCallbackHandler : IClientRelayServiceCallback {
		public bool SendTrackPadMessage(ServiceRequestDetails details, TrackPadMessage message) {
			if (message.Command == TrackPadMessage.TrackPadCommands.DragDelta)
				Eve.API.Touch.TouchProvider.MoveMouse((int)message.X, (int)message.Y);

			return true;
		}

		public bool SendButtonMessage(ServiceRequestDetails details, ButtonMessage message) {
			if (message.Command == ButtonMessage.ButtonCommands.Tap)
				Eve.API.Touch.TouchProvider.ClickButton((TouchProvider.Buttons) message.Button);

			return true;
		}

		public string Ping(string yourName) {
			return "Control" + yourName;
		}

		public bool SignIn(ServiceUser user) {
			throw new System.NotImplementedException();
		}

		public bool SignOut(ServiceUser user) {
			throw new System.NotImplementedException();
		}

		public ServiceClient[] GetAvailableClients() {
			throw new System.NotImplementedException();
		}
	}
}