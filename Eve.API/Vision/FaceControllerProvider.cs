using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eve.Diagnostics.Logging;
using Eve.API.Text;

namespace Eve.API.Vision {
	[ProviderDescription(
		"Face Controller Provider",
		typeof(FaceDetectionProvider))]
	public class FaceControllerProvider : ProviderBase<FaceControllerProvider> {
		// TODO Add smoothing
		// TODO Add center calibration rather than delta move
		private Point center;
		private bool isRequestedCallibrateCenter;
		private int rightOffset;
		private int leftOffset;
		private int topOffset;
		private int bottomOffset;

		private bool isMoveLeftActive;
		private bool isMoveRightActive;
		private bool isMoveTopActive;
		private bool isMoveBottomActive;

		protected override void Initialize() {
			ProviderManager.FaceDetectionProvider.OnFaceMove +=
				this.FaceDetectionProviderOnOnFaceMove;

			// Set initial state to disabled
			this.IsEnabled = false;

			this.center = new Point(
				ProviderManager.VideoProvider.Device.DesiredFrameSize.Width / 2,
				ProviderManager.VideoProvider.Device.DesiredFrameSize.Height / 2);

			this.rightOffset = this.leftOffset = 10;
			this.topOffset = this.bottomOffset = 55;
		}

		protected override void Uninitialize() {
			ProviderManager.FaceDetectionProvider.OnFaceMove -=
				this.FaceDetectionProviderOnOnFaceMove;
		}

		private void FaceDetectionProviderOnOnFaceMove(FaceDetectionProvider provider,
													   FaceDetectionEventArgs args) {
			// Skip calculations if not enabled
			if (!this.IsEnabled) return;

			if (isRequestedCallibrateCenter) {
				this.isRequestedCallibrateCenter = false;
				this.center =
					new Point(args.Tracker.Tracker.TrackingObject.Center.X,
							  args.Tracker.Tracker.TrackingObject.Center.Y);
			}

			int horizontalDelta =
				args.Tracker.Tracker.TrackingObject.Center.X - this.center.X;
			int verticalDelta =
				args.Tracker.Tracker.TrackingObject.Center.Y - this.center.Y;

			this.log.Info("X:{0} Y:{1}", horizontalDelta, verticalDelta);

			if (horizontalDelta > 0 && Math.Abs(horizontalDelta) > leftOffset)
				this.ActivateMoveLeft();
			else this.DeactivateMoveLeft();

			if (horizontalDelta < 0 && Math.Abs(horizontalDelta) > rightOffset)
				this.ActivateMoveRight();
			else this.DeactivateMoveRight();

			if (verticalDelta > 0 && Math.Abs(verticalDelta) > topOffset)
				this.ActivateMoveTop();
			else this.DeactivateMoveTop();

			if (verticalDelta < 0 && Math.Abs(verticalDelta) > bottomOffset)
				this.ActivateMoveBottom();
			else this.DeactivateMoveBottom();
		}

		private void DeactivateMoveRight() {
			if (this.isMoveRightActive)
				ProviderManager.TextProvider.SimulateKeyUp(TextProvider.VirtualKeyCode.RIGHT);
			this.isMoveRightActive = false;
		}

		private void DeactivateMoveTop() {
			if (this.isMoveTopActive)
				ProviderManager.TextProvider.SimulateKeyUp(TextProvider.VirtualKeyCode.UP);
			this.isMoveTopActive = false;
		}

		private void DeactivateMoveBottom() {
			if (this.isMoveBottomActive)
				ProviderManager.TextProvider.SimulateKeyUp(TextProvider.VirtualKeyCode.DOWN);
			this.isMoveBottomActive = false;
		}

		private void DeactivateMoveLeft() {
			if (this.isMoveLeftActive)
				ProviderManager.TextProvider.SimulateKeyUp(TextProvider.VirtualKeyCode.LEFT);
			this.isMoveLeftActive = false;
		}

		private void ActivateMoveTop() {
			if (this.isMoveTopActive) return;

			this.isMoveTopActive = true;
			ProviderManager.TextProvider.SimulateKeyDown(TextProvider.VirtualKeyCode.UP);
		}

		private void ActivateMoveBottom() {
			if (this.isMoveBottomActive) return;

			this.isMoveBottomActive = true;
			ProviderManager.TextProvider.SimulateKeyDown(TextProvider.VirtualKeyCode.DOWN);
		}

		private void ActivateMoveRight() {
			if (this.isMoveRightActive) return;

			this.isMoveRightActive = true;
			ProviderManager.TextProvider.SimulateKeyDown(TextProvider.VirtualKeyCode.RIGHT);
			this.log.Info("Press RIGHT");
		}

		private void ActivateMoveLeft() {
			if (this.isMoveLeftActive) return;

			this.isMoveLeftActive = true;
			ProviderManager.TextProvider.SimulateKeyDown(TextProvider.VirtualKeyCode.LEFT);
			this.log.Info("Press LEFT");
		}

		#region Properties

		public bool IsEnabled { get; set; }

		#endregion

		public void RequestCallibrateCenter() {
			this.isRequestedCallibrateCenter = true;
		}
	}
}
