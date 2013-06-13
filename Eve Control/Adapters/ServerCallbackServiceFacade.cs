using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eve.API;
using Eve.API.Touch;
using Eve.Diagnostics.Logging;
using EveControl.RelayServiceReference;

namespace EveControl.Adapters {
	public class ServerCallbackServiceFacade : IServerCallbackServiceFacade {
		// TODO Check user credentials
		private readonly Log.LogInstance log =
			new Log.LogInstance(typeof(IServerCallbackServiceFacade));

		public bool SignIn(ServiceUser user) {
			// TODO Implement
			throw new System.NotImplementedException();
		}

		public bool SignOut(ServiceUser user) {
			// TODO Implement
			throw new System.NotImplementedException();
		}

		public ServiceClient[] GetAvailableClients() {
			throw new InvalidOperationException("This call shouldn't have been passed to client!");
		}

		#region Touch implementation

		public bool SendTrackPadMessage(ServiceRequestDetails details, TrackPadMessage message) {
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

		public bool SetZoom(ServiceRequestDetails details, int zoomValue) {
			this.log.Info("Handling Zoom request. Value: {0}", zoomValue);

			Task.Run(async () =>
					 await ProviderManager.DisplayEnhancementsProvider.SetZoomAsync(zoomValue));

			return true;
		}

		#endregion

		#region Lights implementation

		public Light[] GetLights(ServiceRequestDetails details) {
			this.log.Info("Handling request for list of lights");

			// TODO Implement real list
			return new Light[] {
				new Light() {
					ID = 0,
					Alias = "Living Room TV Wall",
					State = true
				},
				new Light() {
					ID = 1,
					Alias = "Bedroom Ceiling"
				},
				new Light() {
					ID = 2,
					Alias = "Kitchen LEDs",
					State = true
				}
			};
		}

		public bool SetLightState(ServiceRequestDetails details, int id, bool state) {
			this.log.Info("Handling light [{0}] state set request {1}", id, state);

			// TODO Implement
			return true;
		}

		#endregion

		#region AmbientalLights implementation

		public AmbientalLight[] GetAmbientalLights(ServiceRequestDetails details) {
			this.log.Info("Handling request for list of ambiental lights");

			// TODO Implement real list
			return new AmbientalLight[] {
				new AmbientalLight() {
					ID = 0, Alias = "Living Room Ceiling",
					RValue = 255, GValue = 0, BValue = 0, AValue = 255,
					State = true
				},
				new AmbientalLight() {
					ID = 1, Alias = "Bedroom Ceiling",
					RValue = 0, GValue = 255, BValue = 0, AValue = 255,
					State = true
				},
				new AmbientalLight() {
					ID = 2, Alias = "Kitchen Ceiling",
					RValue = 0, GValue = 0, BValue = 255, AValue = 255
				},
				new AmbientalLight() {
					ID = 3, Alias = "TV Backlight",
					RValue = 0, GValue = 255, BValue = 255, AValue = 255
				},
				new AmbientalLight() {
					ID = 4, Alias = "Workdesk backlight",
					RValue = 255, GValue = 0, BValue = 255, AValue = 255,
					State = true
				}
			};
		}

		public bool SetAmbientalLightState(ServiceRequestDetails details, int id, bool state) {
			this.log.Info("Handling ambiental light [{0}] state set request: {1}",
						  id, state ? "On" : "Off");

			// TODO Implement
			return true;
		}

		public bool SetAmbientalLightColor(ServiceRequestDetails details,
			int id, byte r, byte g, byte b, byte a) {
			this.log.Info(
				"Handling ambiental light [{0}] color set request: RGB:{1:X}{2:X}{3:X} A:{4:X}",
				id, r, g, b, a);

			// TODO Implement
			return true;
		}

		#endregion
	}
}
