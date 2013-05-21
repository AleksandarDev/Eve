using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge;
using Eve.Diagnostics.Logging;
using Eve.API.Text;

namespace Eve.API.Vision {
	[ProviderDescription(
		"Face Controller Provider",
		typeof(FaceDetectionProvider))]
	public class FaceControllerProvider : ProviderBase<FaceControllerProvider> {
		// TODO Add smoothing
		// TODO Add center calibration rather than delta move
		private int xOffsetCurrent = 0;
		private int yOffsetCurrent = 0;
		private int areaOffsetCurrent = 0;

		protected override void Initialize() {
			ProviderManager.FaceDetectionProvider.OnFaceMove +=
				this.FaceDetectionProviderOnOnFaceMove;

			// Set initial state to disabled
			this.IsEnabled = false;
		}

		protected override void Uninitialize() {
			ProviderManager.FaceDetectionProvider.OnFaceMove -=
				this.FaceDetectionProviderOnOnFaceMove;
		}

		private void FaceDetectionProviderOnOnFaceMove(FaceDetectionProvider provider,
													   FaceDetectionEventArgs args) {
			// Skip calculations if not enabled
			if (!this.IsEnabled) return;

			var faceCenter = args.Tracker.Tracker.TrackingObject.Center;
			int xOffset = ProviderManager.VideoProvider.SourceWidth / 2 -
						  faceCenter.X - this.xOffsetCurrent;
			int yOffset = ProviderManager.VideoProvider.SourceHeight / 2 -
						  faceCenter.Y - this.yOffsetCurrent;
			int areaOffset = ProviderManager.VideoProvider.SourceWidth *
							 ProviderManager.VideoProvider.SourceHeight / 8 -
							 args.Tracker.Tracker.TrackingObject.Area - this.areaOffsetCurrent;


			this.log.Info("Face offset X:{0} Z:{1}", xOffset, areaOffset);

			if (xOffset < 0)
				for (int index = 0; index < Math.Abs(xOffset); index++)
					ProviderManager.TouchProvider.MoveMouse(-1, 0);
			if (xOffset > 0)
				for (int index = 0; index < Math.Abs(xOffset); index++)
					ProviderManager.TouchProvider.MoveMouse(1, 0);
			if (areaOffset < 0)
				for (int index = 0; index < Math.Abs(areaOffset) / 100; index++)
					ProviderManager.TouchProvider.MoveMouse(0, -1);
			if (areaOffset > 0)
				for (int index = 0; index < Math.Abs(areaOffset) / 100; index++)
					ProviderManager.TouchProvider.MoveMouse(0, 1);

			this.xOffsetCurrent = ProviderManager.VideoProvider.SourceWidth / 2 -
								  faceCenter.X;
			this.yOffsetCurrent = ProviderManager.VideoProvider.SourceHeight / 2 -
								  faceCenter.Y;
			this.areaOffsetCurrent = ProviderManager.VideoProvider.SourceWidth *
									 ProviderManager.VideoProvider.SourceHeight / 8 -
									 args.Tracker.Tracker.TrackingObject.Area;
		}


		#region Properties

		public bool IsEnabled { get; set; }

		#endregion
	}
}
