using System;
using Eve.API;
using Eve.API.Services.Common;
using Eve.API.Services.Common.Modules.Touch;
using Eve.API.Touch;
using Eve.Diagnostics.Logging;
using EveControl.RelayServiceReference;

namespace EveControl.Communication {
	/// <summary>
	/// Implementation of Relay Service Callback methods
	/// </summary>
	public class RelayServiceCallbackHandler : IClientRelayServiceCallback {
		private readonly Log.LogInstance log =
			new Log.LogInstance(typeof(RelayServiceCallbackHandler));

		public bool SignIn(ServiceUser user) {
			throw new System.NotImplementedException();
		}

		public bool SignOut(ServiceUser user) {
			throw new System.NotImplementedException();
		}

		public ServiceClient[] GetAvailableClients() {
			throw new InvalidOperationException("This call shouldn't have been passed to client!");
		}

		#region Touch implementation

		public bool SendTrackPadMessage(ServiceRequestDetails details, TrackPadMessage message) {
			// TODO Check user credentials
			this.log.Info("Handling Touch TrackPad message: {0}", message);

			if (message.Command == TrackPadMessage.TrackPadCommands.DragDelta)
				ProviderManager.TouchProvider.MoveMouseSmart(message.X, message.Y);
			else if (message.Command == TrackPadMessage.TrackPadCommands.Tap)
				ProviderManager.TouchProvider.ClickButton(TouchProvider.Buttons.Left);
			else if (message.Command == TrackPadMessage.TrackPadCommands.DoubleTap) {
				ProviderManager.TouchProvider.ClickButton(TouchProvider.Buttons.Left);
				ProviderManager.TouchProvider.ClickButton(TouchProvider.Buttons.Left);
			}

			return true;
		}

		public bool SendButtonMessage(ServiceRequestDetails details, ButtonMessage message) {
			// TODO Check user credentials
			this.log.Info("Handling Touch Button message: {0}", message);

			if (message.Command == ButtonMessage.ButtonCommands.Tap)
				ProviderManager.TouchProvider.ClickButton((TouchProvider.Buttons)message.Button);
			else if (message.Command == ButtonMessage.ButtonCommands.DoubleTap) {
				ProviderManager.TouchProvider.ClickButton((TouchProvider.Buttons)message.Button);
				ProviderManager.TouchProvider.ClickButton((TouchProvider.Buttons)message.Button);
			} else if (message.Command == ButtonMessage.ButtonCommands.Hold) {
				this.log.Warn("HOLD on button isn't implemented yet");
			}

			return true;
		}

		#endregion

		#region Display Enhancements implementation
		#endregion

		public bool SetZoom(ServiceRequestDetails details, int zoomValue) {
			// TODO Check user credentials
			this.log.Info("Handling Zoom request. Value: {0}", zoomValue);

			// This can be run async because there is no other 
			// instruction below this code and returns void
			#pragma warning disable 4014
			ProviderManager.DisplayEnhancementsProvider.SetZoomAsync(zoomValue);
			#pragma warning restore 4014

			return true;
		}
	}
}