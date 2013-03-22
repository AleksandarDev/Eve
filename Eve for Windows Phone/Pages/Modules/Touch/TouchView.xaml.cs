using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TrackPadCommands = EveWindowsPhone.Pages.Modules.Touch.TouchViewModel.TrackPadMessage.TrackPadCommands;
using ButtonCommands = EveWindowsPhone.Pages.Modules.Touch.TouchViewModel.ButtonMessage.ButtonCommands;

namespace EveWindowsPhone.Pages.Modules.Touch {
	public partial class TouchView : PhoneApplicationPage {
		public TouchView() {
			InitializeComponent();

			// Assign ViewModel
			if (this.ViewModel == null)
				if ((this.ViewModel = this.DataContext as TouchViewModel) == null)
					throw new NullReferenceException("Invalid ViewModel");
		}

		private void TouchViewLoaded(object sender, RoutedEventArgs e) {
			this.AssignGestures();
		}

		private void AssignGestures() {
			// Retrieve gesture listeners
			var trackPadGestureListener = GestureService.GetGestureListener(this.TrackPad);
			var rightButtonGestureListener = GestureService.GetGestureListener(this.RightButton);
			var leftButtonGestureListener = GestureService.GetGestureListener(this.LeftButton);

			// TrackPad Gestures
			trackPadGestureListener.Tap +=
				(s, es) => this.ViewModel.OnTrackPadGesture(TrackPadCommands.Tap, es);
			trackPadGestureListener.Hold +=
				(s, es) => this.ViewModel.OnTrackPadGesture(TrackPadCommands.Hold, es);
			trackPadGestureListener.DoubleTap +=
				(s, es) => this.ViewModel.OnTrackPadGesture(TrackPadCommands.DoubleTap, es);
			trackPadGestureListener.Flick +=
				(s, es) => this.ViewModel.OnTrackPadGesture(TrackPadCommands.Flick, es);
			trackPadGestureListener.DragStarted +=
				(s, es) => this.ViewModel.OnTrackPadGesture(TrackPadCommands.DragStarted, es);
			trackPadGestureListener.DragDelta +=
				(s, es) => this.ViewModel.OnTrackPadGesture(TrackPadCommands.DragDelta, es);
			trackPadGestureListener.DragCompleted +=
				(s, es) => this.ViewModel.OnTrackPadGesture(TrackPadCommands.DragCompleted, es);
			trackPadGestureListener.PinchStarted +=
				(s, es) => this.ViewModel.OnTrackPadGesture(TrackPadCommands.PinchStarted, es);
			trackPadGestureListener.PinchDelta +=
				(s, es) => this.ViewModel.OnTrackPadGesture(TrackPadCommands.PinchDelta, es);
			trackPadGestureListener.PinchCompleted +=
				(s, es) => this.ViewModel.OnTrackPadGesture(TrackPadCommands.PinchCompleted, es);

			// LeftButton Gestures
			leftButtonGestureListener.Tap +=
				(s, es) => this.ViewModel.OnButtonGesture(ButtonCommands.Tap);
			leftButtonGestureListener.Hold +=
				(s, es) => this.ViewModel.OnButtonGesture(ButtonCommands.Hold);
			leftButtonGestureListener.DoubleTap +=
				(s, es) => this.ViewModel.OnButtonGesture(ButtonCommands.DoubleTap);

			// RightButtonGestures
			rightButtonGestureListener.Tap +=
				(s, es) => this.ViewModel.OnButtonGesture(ButtonCommands.Tap);
			rightButtonGestureListener.Hold +=
				(s, es) => this.ViewModel.OnButtonGesture(ButtonCommands.Hold);
			rightButtonGestureListener.DoubleTap +=
				(s, es) => this.ViewModel.OnButtonGesture(ButtonCommands.DoubleTap);
		}


		#region Properties

		public TouchViewModel ViewModel { get; private set; }

		#endregion
	}
}