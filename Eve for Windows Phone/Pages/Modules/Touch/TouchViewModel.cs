using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Eve.API.Services.Contracts;
using EveWindowsPhone.Adapters;
using EveWindowsPhone.Modules;
using EveWindowsPhone.RelayServiceReference;
using EveWindowsPhone.ViewModels;
using Microsoft.Phone.Controls;

namespace EveWindowsPhone.Pages.Modules.Touch {
	[Module("MTouch", "Touch", "/Resources/Images/Touch screens.png", "/Pages/Modules/Touch/TouchView.xaml")]
	public class TouchViewModel : NotificationObject {
		private readonly INavigationServiceFacade navigationServiceFacade;
		private readonly IIsolatedStorageServiceFacade isolatedStorageServiceFacade;
		//private TouchServiceClient serviceClient;


		public TouchViewModel(INavigationServiceFacade navigationServiceFacade,
		                      IIsolatedStorageServiceFacade isolatedStorageServiceFacade) {
			if (navigationServiceFacade == null) throw new ArgumentNullException("navigationServiceFacade");
			this.navigationServiceFacade = navigationServiceFacade;
			this.isolatedStorageServiceFacade = isolatedStorageServiceFacade;

			//this.serviceClient = new TouchServiceClient();
		}


		public void OnTrackPadGesture<T>(TrackPadMessage.TrackPadCommands command,
										 T eventArgument) {
			var message = this.ConstructTrackPadMessage<T>(command, eventArgument);
			if (message == null) {
				System.Diagnostics.Debug.WriteLine("Invalid gesture for TrackPad");
				return;
			}

			System.Diagnostics.Debug.WriteLine(message.ToString());

			EveWindowsPhone.Pages.Main.MainView.client.SendTrackPadMessageAsync(
				new ServiceRequestDetails() {
					Client =
						new ServiceClient() {
							Alias = "Aleksandar Toplek Laptop",
							ID = "AleksandarPC"
						}
				},
				message);
			//this.serviceClient.ProcessTrackPadMessageAsync("TestToken", message);
		}

		public void OnButtonGesture(ButtonMessage.Buttons button,
									ButtonMessage.ButtonCommands command) {
			var message = this.ConstructButtonMessage(button, command);

			System.Diagnostics.Debug.WriteLine(message.ToString());

			EveWindowsPhone.Pages.Main.MainView.client.SendButtonMessageAsync(
				new ServiceRequestDetails() {
					Client =
						new ServiceClient() {
							Alias = "Aleksandar Toplek Laptop",
							ID = "AleksandarPC"
						}
				},
				message);
			//this.serviceClient.ProcessButtonMessageAsync("TestToken", message);
		}


		private TrackPadMessage ConstructTrackPadMessage<T>(TrackPadMessage.TrackPadCommands command, T eventArgument) {
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

		private TrackPadMessage ConstructTrackPadPinchMessage(TrackPadMessage.TrackPadCommands command, PinchGestureEventArgs args) {
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

		private TrackPadMessage ConstructTrackPadMessageSimple(TrackPadMessage.TrackPadCommands command) {
			return new TrackPadMessage() {Command = command};
		}

		private ButtonMessage ConstructButtonMessage(ButtonMessage.Buttons button, ButtonMessage.ButtonCommands command) {
			return new ButtonMessage() {Button = button, Command = command};
		}
	}
}
