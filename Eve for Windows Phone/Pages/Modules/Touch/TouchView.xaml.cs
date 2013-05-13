using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Eve.API.Services.Contracts;
using EveWindowsPhone.RelayServiceReference;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace EveWindowsPhone.Pages.Modules.Touch {
	public partial class TouchView : PhoneApplicationPage {
		// TODO Implement multi touch 
		// TODO Two finger scroll
		// TODO Three finger swipe

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

		protected override void OnNavigatedFrom(NavigationEventArgs e) {
			this.ViewModel.NavigatedFrom(e);
			
			base.OnNavigatedFrom(e);
		}

		private void AssignGestures() {
			// Retrieve gesture listeners
			var trackPadGestureListener = GestureService.GetGestureListener(this.TrackPad);
			var rightButtonGestureListener = GestureService.GetGestureListener(this.RightButton);
			var leftButtonGestureListener = GestureService.GetGestureListener(this.LeftButton);

			// TrackPad Gestures
			trackPadGestureListener.Tap +=
				(s, es) => this.ViewModel.OnTrackPadGesture(TrackPadMessage.TrackPadCommands.Tap, es);
			trackPadGestureListener.Hold +=
				(s, es) => this.ViewModel.OnTrackPadGesture(TrackPadMessage.TrackPadCommands.Hold, es);
			trackPadGestureListener.DoubleTap +=
				(s, es) => this.ViewModel.OnTrackPadGesture(TrackPadMessage.TrackPadCommands.DoubleTap, es);
			trackPadGestureListener.Flick +=
				(s, es) => this.ViewModel.OnTrackPadGesture(TrackPadMessage.TrackPadCommands.Flick, es);
			trackPadGestureListener.DragStarted +=
				(s, es) => this.ViewModel.OnTrackPadGesture(TrackPadMessage.TrackPadCommands.DragStarted, es);
			trackPadGestureListener.DragDelta +=
				(s, es) => this.ViewModel.OnTrackPadGesture(TrackPadMessage.TrackPadCommands.DragDelta, es);
			trackPadGestureListener.DragCompleted +=
				(s, es) => this.ViewModel.OnTrackPadGesture(TrackPadMessage.TrackPadCommands.DragCompleted, es);
			trackPadGestureListener.PinchStarted +=
				(s, es) => this.ViewModel.OnTrackPadGesture(TrackPadMessage.TrackPadCommands.PinchStarted, es);
			trackPadGestureListener.PinchDelta +=
				(s, es) => this.ViewModel.OnTrackPadGesture(TrackPadMessage.TrackPadCommands.PinchDelta, es);
			trackPadGestureListener.PinchCompleted +=
				(s, es) => this.ViewModel.OnTrackPadGesture(TrackPadMessage.TrackPadCommands.PinchCompleted, es);

			// LeftButton Gestures
			leftButtonGestureListener.Tap +=
				(s, es) => this.ViewModel.OnButtonGesture(ButtonMessage.Buttons.Left, ButtonMessage.ButtonCommands.Tap);
			leftButtonGestureListener.Hold +=
				(s, es) => this.ViewModel.OnButtonGesture(ButtonMessage.Buttons.Left, ButtonMessage.ButtonCommands.Hold);
			leftButtonGestureListener.DoubleTap +=
				(s, es) => this.ViewModel.OnButtonGesture(ButtonMessage.Buttons.Left, ButtonMessage.ButtonCommands.DoubleTap);

			// RightButtonGestures
			rightButtonGestureListener.Tap +=
				(s, es) => this.ViewModel.OnButtonGesture(ButtonMessage.Buttons.Right, ButtonMessage.ButtonCommands.Tap);
			rightButtonGestureListener.Hold +=
				(s, es) => this.ViewModel.OnButtonGesture(ButtonMessage.Buttons.Right, ButtonMessage.ButtonCommands.Hold);
			rightButtonGestureListener.DoubleTap +=
				(s, es) => this.ViewModel.OnButtonGesture(ButtonMessage.Buttons.Right, ButtonMessage.ButtonCommands.DoubleTap);
		}


		#region Properties

		public TouchViewModel ViewModel { get; private set; }

		#endregion

		private void SettingsClick(object sender, EventArgs e) {
			this.ViewModel.AdvancedSettings();
		}

		private void HelpClick(object sender, EventArgs e) {
			(new WebBrowserTask() {
				Uri = new Uri("http://eve.toplek.net/help/module/touch/", UriKind.Absolute)
			}).Show();
		}
	}
}