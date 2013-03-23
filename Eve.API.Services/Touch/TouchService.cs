using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Eve.API.Services.Common;
using Eve.API.Touch;
using Eve.Core;

namespace Eve.API.Services.Touch {
	[ServiceBehavior(
		InstanceContextMode = InstanceContextMode.Single,
		ConcurrencyMode = ConcurrencyMode.Multiple)]
	public sealed class TouchService : ITouchService {
		public TouchService() {
			System.Diagnostics.Debug.WriteLine(String.Format("Touch Service object created [{0}]", this.GetHashCode()));

			if (!TouchProvider.IsRuning)
				Task.Run(async delegate { await TouchProvider.Start(); });
		}

		~TouchService() {
			System.Diagnostics.Debug.WriteLine(String.Format("Touch Service object destroyed [{0}]", this.GetHashCode()));
		}


		public bool ProcessTrackPadMessage(string token, TrackPadMessage message) {
			// TODo Check if token is valid

			System.Diagnostics.Debug.WriteLine(String.Format("From [{0}] got: {1}", token, message.ToString()));

			switch (message.Command) {
				case TrackPadMessage.TrackPadCommands.Tap:
					TouchProvider.ClickButton(Eve.API.Touch.TouchProvider.Buttons.Left);
					return true;
				case TrackPadMessage.TrackPadCommands.Hold:
					return true;
				case TrackPadMessage.TrackPadCommands.DoubleTap:
					TouchProvider.ClickButton(Eve.API.Touch.TouchProvider.Buttons.Left);
					TouchProvider.ClickButton(Eve.API.Touch.TouchProvider.Buttons.Left);
					return true;
				case TrackPadMessage.TrackPadCommands.Flick:
					return true;
				case TrackPadMessage.TrackPadCommands.DragStarted:
					return true;
				case TrackPadMessage.TrackPadCommands.DragDelta:
					TouchProvider.MoveMouse((int)message.X, (int)message.Y);
					return true;
				case TrackPadMessage.TrackPadCommands.DragCompleted:
					//TouchProvider.MoveMouse((int)message.X, (int)message.Y);
					return true;
				case TrackPadMessage.TrackPadCommands.PinchStarted:
					return true;
				case TrackPadMessage.TrackPadCommands.PinchDelta:
					return true;
				case TrackPadMessage.TrackPadCommands.PinchCompleted:
					return true;
				default:
					return false;
			}
		}

		public bool ProcessButtonMessage(string token, ButtonMessage message) {
			// TODO Check user token

			System.Diagnostics.Debug.WriteLine(String.Format("From [{0}] got: {1}", token, message.ToString()));

			switch (message.Command) {
				case ButtonMessage.ButtonCommands.Tap:
					TouchProvider.ClickButton((Eve.API.Touch.TouchProvider.Buttons)message.Button);
					return true;
				case ButtonMessage.ButtonCommands.Hold:
					System.Diagnostics.Debug.WriteLine("Implement Touch Button Hold");
					// TODO Implement
					return true;
				case ButtonMessage.ButtonCommands.DoubleTap:
					TouchProvider.ClickButton((Eve.API.Touch.TouchProvider.Buttons)message.Button);
					TouchProvider.ClickButton((Eve.API.Touch.TouchProvider.Buttons)message.Button);
					return true;
				default:
					return false;
			}
		}

		public string SignIn(string userName, string passwordHash) {
			throw new NotImplementedException();
		}

		public void SignOut(string token) {
			throw new NotImplementedException();
		}
	}
}
