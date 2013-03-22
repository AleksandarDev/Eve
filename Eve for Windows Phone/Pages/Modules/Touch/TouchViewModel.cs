using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using EveWindowsPhone.Adapters;
using EveWindowsPhone.Modules;
using EveWindowsPhone.ViewModels;
using Microsoft.Phone.Controls;

namespace EveWindowsPhone.Pages.Modules.Touch {
	[Module("Touch", "/Resources/Images/Touch screens.png", "/Pages/Modules/Touch/TouchView.xaml")]
	public class TouchViewModel : NotificationObject {
		private INavigationServiceFacade navigationServiceFacade;


		public TouchViewModel(INavigationServiceFacade navigationServiceFacade) {
			if (navigationServiceFacade == null) throw new ArgumentNullException("navigationServiceFacade");
			this.navigationServiceFacade = navigationServiceFacade;
		}


		public void OnTrackPadGesture<T>(TrackPadMessage.TrackPadCommands command, T eventArgument) {
			var message = this.ConstructTrackPadMessage<T>(command, eventArgument);
			if (message == null) {
				System.Diagnostics.Debug.WriteLine("Invalid gesture for TrackPad");
				return;
			}

			System.Diagnostics.Debug.WriteLine(message.ToString());
		}

		public void OnButtonGesture(ButtonMessage.ButtonCommands command) {
			var message = this.ConstructButtonMessage(command);

			System.Diagnostics.Debug.WriteLine(message.ToString());
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
			return new TrackPadMessage(command,
			                           ratio: args.DistanceRatio, angle: args.TotalAngleDelta);
		}

		private TrackPadMessage ConstructTrackPadMessage(PinchStartedGestureEventArgs args) {
			return new TrackPadMessage(TrackPadMessage.TrackPadCommands.PinchStarted,
									   ratio: args.Distance, angle: args.Angle);
		}

		private TrackPadMessage ConstructTrackPadMessage(DragCompletedGestureEventArgs args) {
			return new TrackPadMessage(TrackPadMessage.TrackPadCommands.DragCompleted,
			                           direction: args.Direction, x: args.HorizontalChange, y: args.VerticalChange);
		}

		private TrackPadMessage ConstructTrackPadMessage(DragDeltaGestureEventArgs args) {
			return new TrackPadMessage(TrackPadMessage.TrackPadCommands.DragDelta,
			                           direction: args.Direction, x: args.HorizontalChange, y: args.VerticalChange);
		}

		private TrackPadMessage ConstructTrackPadMessage(DragStartedGestureEventArgs args) {
			return new TrackPadMessage(TrackPadMessage.TrackPadCommands.DragStarted,
			                           direction: args.Direction);
		}

		private TrackPadMessage ConstructTrackPadMessage(FlickGestureEventArgs args) {
			return new TrackPadMessage(TrackPadMessage.TrackPadCommands.Flick,
			                           angle: args.Angle, direction: args.Direction, x: args.HorizontalVelocity,
			                           y: args.VerticalVelocity);
		}

		private TrackPadMessage ConstructTrackPadMessageSimple(TrackPadMessage.TrackPadCommands command) {
			return new TrackPadMessage(command);
		}

		private ButtonMessage ConstructButtonMessage(ButtonMessage.ButtonCommands command) {
			return new ButtonMessage(command);
		}


		public class TrackPadMessage {
			public TrackPadMessage(TrackPadCommands command,
			                       double x = default(double), double y = default(double),
			                       Orientation direction = default(Orientation),
			                       double angle = default(double), double ratio = default(double)) {
				this.Command = command;
				this.X = x;
				this.Y = y;
				this.Direction = direction;
				this.Angle = angle;
				this.DistanceRatio = ratio;
			}


			public override string ToString() {
				return String.Format("Command: {0}  |  (X,Y): ({1},{2})  |  Direction: {3}  |  Angle: {4}  |  Ratio: {5}",
				                     this.Command.ToString(), this.X, this.Y, this.Direction.ToString(), this.Angle,
				                     this.DistanceRatio);
			}

			#region Properties

			public TrackPadCommands Command { get; set; }

			/// <summary>
			/// Gets and sets X axis of command
			/// HorizontalChange for Drag
			/// HorizontalVelocity for Flick
			/// </summary>
			public double X { get; set; }

			/// <summary>
			/// Gets and sets Y axis of command
			/// VerticalChange for Drag
			/// VerticalVelocity for Flick
			/// </summary>
			public double Y { get; set; }

			/// <summary>
			/// Gets and sets direction of command
			/// </summary>
			public Orientation Direction { get; set; }

			/// <summary>
			/// Gets or sets angle of command
			/// Angle for Flick
			/// TotalAngleDelta for Pinch
			/// Angle for PinchStarted
			/// </summary>
			public double Angle { get; set; }

			/// <summary>
			/// Gets and sets distance ratio for command
			/// DistanceRatio for Pinch
			/// Distance for PichStarted
			/// </summary>
			public double DistanceRatio { get; set; }

			#endregion


			public enum TrackPadCommands {
				Tap,
				Hold,
				DoubleTap,
				Flick,
				DragStarted,
				DragDelta,
				DragCompleted,
				PinchStarted,
				PinchDelta,
				PinchCompleted
			}
		}

		public class ButtonMessage {
			public ButtonMessage(ButtonCommands command) {
				this.Command = command;
			}


			public override string ToString() {
				return String.Format("Command: {0}", this.Command);
			}

			#region Properties

			public ButtonCommands Command { get; set; }

			#endregion


			public enum ButtonCommands {
				Tap,
				Hold,
				DoubleTap
			}
		}
	}
}
