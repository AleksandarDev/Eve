using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows.Navigation;
using Eve.API.Services.Contracts;
using Eve.Diagnostics.Logging;
using EveWindowsPhone.Adapters;
using EveWindowsPhone.Modules;
using EveWindowsPhone.RelayServiceReference;
using EveWindowsPhone.ViewModels;
using Microsoft.Phone.Controls;

namespace EveWindowsPhone.Pages.Modules.Touch {
	[Module("MTouch", "Touch", "/Resources/Images/Touch screens.png", "/Pages/Modules/Touch/TouchView.xaml")]
	public class TouchViewModel : NotificationObject {
		private readonly Log.LogInstance log =
			new Log.LogInstance(typeof(TouchViewModel));

		private readonly INavigationServiceFacade navigationServiceFacade;
		private readonly IIsolatedStorageServiceFacade isolatedStorageServiceFacade;
		private readonly IRelayServiceFacade relayServiceFacade;

		private bool isZoomEnabled;
		private int zoomAmount;


		public TouchViewModel(INavigationServiceFacade navigationServiceFacade,
							  IIsolatedStorageServiceFacade isolatedStorageServiceFacade,
							  IRelayServiceFacade relayServiceFacade) {
			if (navigationServiceFacade == null)
				throw new ArgumentNullException("navigationServiceFacade");
			if (isolatedStorageServiceFacade == null)
				throw new ArgumentNullException("isolatedStorageServiceFacade");
			if (relayServiceFacade == null)
				throw new ArgumentNullException("relayServiceFacade");

			this.navigationServiceFacade = navigationServiceFacade;
			this.isolatedStorageServiceFacade = isolatedStorageServiceFacade;
			this.relayServiceFacade = relayServiceFacade;

			this.LoadSettings();

			// Send zoom request if enabled
			if (this.isZoomEnabled) {
				// TODO SetZoom relay request
				this.relayServiceFacade.Proxy.Relay.SetZoomAsync(
					this.relayServiceFacade.Proxy.ActiveDetails, this.zoomAmount);
				this.log.Info("Zoom set to {0}", this.zoomAmount);
			}

			this.log.Info("View model created");
		}

		private void LoadSettings() {
			// Check if zoom is enabled
			this.isZoomEnabled = this.isolatedStorageServiceFacade.GetSetting<bool>(
				IsolatedStorageServiceFacade.ActivateZoomOnTouchKey);

			// If zoom is enabled, get zoom amount value
			if (this.isZoomEnabled) {
				this.zoomAmount = this.isolatedStorageServiceFacade.GetSetting<int>(
					IsolatedStorageServiceFacade.ActivateZoomOnTouchValueKey);

				this.log.Info("Zoom is enabled");
				this.log.Info("Zoom amout se to [{0}]", this.zoomAmount);
			}
		}

		#region Message handling

		public void OnTrackPadGesture<T>(
			TrackPadMessage.TrackPadCommands command, T eventArgument) {
			var message = this.ConstructTrackPadMessage<T>(command, eventArgument);
			if (message == null) {
				System.Diagnostics.Debug.WriteLine("Invalid gesture for TrackPad");
				return;
			}

			this.log.Info("Sending track pad message: {0}", message.ToString());

			// Send request for track pad message to client
			this.relayServiceFacade.Proxy.Relay.SendTrackPadMessageAsync(
				this.relayServiceFacade.Proxy.ActiveDetails, message);
		}

		public void OnButtonGesture(
			ButtonMessage.Buttons button, ButtonMessage.ButtonCommands command) {
			var message = this.ConstructButtonMessage(button, command);

			this.log.Info("Sending button message: {0}", message.ToString());

			// Send request for button message to client
			this.relayServiceFacade.Proxy.Relay.SendButtonMessageAsync(
				this.relayServiceFacade.Proxy.ActiveDetails, message);
		}


		private TrackPadMessage ConstructTrackPadMessage<T>(
			TrackPadMessage.TrackPadCommands command, T eventArgument) {
			// Process simple message
			if (command == TrackPadMessage.TrackPadCommands.Tap ||
			    command == TrackPadMessage.TrackPadCommands.DoubleTap ||
			    command == TrackPadMessage.TrackPadCommands.Hold)
				return this.ConstructTrackPadMessageSimple(command);

			// Flick gesture
			if (command == TrackPadMessage.TrackPadCommands.Flick)
				return this.ConstructTrackPadMessage(eventArgument as FlickGestureEventArgs);

			// Drag gestures
			if (command == TrackPadMessage.TrackPadCommands.DragStarted)
				return this.ConstructTrackPadMessage(eventArgument as DragStartedGestureEventArgs);
			if (command == TrackPadMessage.TrackPadCommands.DragDelta)
				return this.ConstructTrackPadMessage(eventArgument as DragDeltaGestureEventArgs);
			if (command == TrackPadMessage.TrackPadCommands.DragCompleted)
				return this.ConstructTrackPadMessage(eventArgument as DragCompletedGestureEventArgs);

			// Pinch gesture
			if (command == TrackPadMessage.TrackPadCommands.PinchStarted)
				return this.ConstructTrackPadMessage(eventArgument as PinchStartedGestureEventArgs);
			if (command == TrackPadMessage.TrackPadCommands.PinchDelta ||
				command == TrackPadMessage.TrackPadCommands.PinchCompleted)
				return this.ConstructTrackPadPinchMessage(command, eventArgument as PinchGestureEventArgs);

			return null;
		}

		#region Message constructors

		private TrackPadMessage ConstructTrackPadPinchMessage(
			TrackPadMessage.TrackPadCommands command, PinchGestureEventArgs args) {
			return new TrackPadMessage() {
				Command = command,
				DistanceRatio = args.DistanceRatio,
				Angle = args.TotalAngleDelta
			};
		}

		private TrackPadMessage ConstructTrackPadMessage(PinchStartedGestureEventArgs args) {
			return new TrackPadMessage() {
				Command = TrackPadMessage.TrackPadCommands.PinchStarted,
				DistanceRatio = args.Distance,
				Angle = args.Angle
			};
		}

		private TrackPadMessage ConstructTrackPadMessage(DragCompletedGestureEventArgs args) {
			return new TrackPadMessage() {
				Command = TrackPadMessage.TrackPadCommands.DragCompleted,
				Direction = (Orientation) args.Direction,
				X = args.HorizontalChange,
				Y = args.VerticalChange
			};
		}

		private TrackPadMessage ConstructTrackPadMessage(DragDeltaGestureEventArgs args) {
			return new TrackPadMessage() {
				Command = TrackPadMessage.TrackPadCommands.DragDelta,
				Direction = (Orientation) args.Direction,
				X = args.HorizontalChange,
				Y = args.VerticalChange
			};
		}

		private TrackPadMessage ConstructTrackPadMessage(DragStartedGestureEventArgs args) {
			return new TrackPadMessage() {
				Command = TrackPadMessage.TrackPadCommands.DragStarted,
				Direction = (Orientation) args.Direction
			};
		}

		private TrackPadMessage ConstructTrackPadMessage(FlickGestureEventArgs args) {
			return new TrackPadMessage() {
				Command = TrackPadMessage.TrackPadCommands.Flick,
				Angle = args.Angle,
				Direction = (Orientation) args.Direction,
				X = args.HorizontalVelocity,
				Y = args.VerticalVelocity
			};
		}

		private TrackPadMessage ConstructTrackPadMessageSimple(
			TrackPadMessage.TrackPadCommands command) {
			return new TrackPadMessage() {Command = command};
		}

		private ButtonMessage ConstructButtonMessage(
			ButtonMessage.Buttons button, ButtonMessage.ButtonCommands command) {
			return new ButtonMessage() {Button = button, Command = command};
		}

		#endregion

		#endregion

		public void NavigatedFrom(NavigationEventArgs e) {
			// Deactivate zoom 
			if (this.isZoomEnabled) {
				this.relayServiceFacade.Proxy.Relay.SetZoomAsync(
					this.relayServiceFacade.Proxy.ActiveDetails, 100);
			}
		}
	}
}
