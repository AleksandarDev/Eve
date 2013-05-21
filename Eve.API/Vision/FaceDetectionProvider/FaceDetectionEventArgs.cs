using System;

namespace Eve.API.Vision {
	public class FaceDetectionEventArgs : EventArgs {
		public FaceDetectionProvider.FaceTracker Tracker;

		public FaceDetectionEventArgs() : base() {}
	}
}