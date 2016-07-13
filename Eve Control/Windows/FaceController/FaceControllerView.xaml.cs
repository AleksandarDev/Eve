using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using Eve.API;
using MahApps.Metro.Controls;
using Size = System.Drawing.Size;
using Accord.Imaging;
using Accord.Controls;

namespace EveControl.Windows.FaceController {
	/// <summary>
	/// Interaction logic for FaceControllerView.xaml
	/// </summary>
	public partial class FaceControllerView : MetroWindow {
		private readonly Eve.Diagnostics.Logging.Log.LogInstance log = new
	Eve.Diagnostics.Logging.Log.LogInstance(typeof(FaceControllerView));

		private Size videoPlayerSize = new Size(320, 240);
		private VideoSourcePlayer videoSourcePlayer;


		public FaceControllerView() {
			InitializeComponent();
		}

		private void FaceControllerViewOnLoaded(object sender, RoutedEventArgs e) {
			this.LoadVideoPlayer();

			this.SetStatus("Ready");
		}

		private void LoadVideoPlayer() {
			this.SetStatus("Creating video viewer...");

			this.videoSourcePlayer = new VideoSourcePlayer() {
				Size = this.videoPlayerSize,
				VideoSource = ProviderManager.VideoProvider.DeviceVideoSource
			};
			this.videoSourcePlayer.NewFrame += HandleNewVideoFrame;
			this.VideoSourcePlayerWindowsFormsHost.Child = this.videoSourcePlayer;

			this.SetStatus("Video player successfully created");
		}

		private void HandleNewVideoFrame(object sender, ref Bitmap image) {
			if (ProviderManager.FaceDetectionProvider == null ||
				!ProviderManager.FaceDetectionProvider.IsRunning) {
				this.log.Warn("Can't process frame. Start face detection provider first");
				return;
			}

			var unmanagedImage = UnmanagedImage.FromManagedImage(image);

			foreach (var tracker in ProviderManager.FaceDetectionProvider.Trackers) {
				tracker.DrawMarker(ref unmanagedImage, true);
			}

			image = unmanagedImage.ToManagedImage();
			unmanagedImage.Dispose();
		}

		private void FaceControllerViewClosing(object sender, CancelEventArgs e) {
			this.videoSourcePlayer.Dispose();
		}

		private void SetStatus(string message) {
			this.StatusLabel.Dispatcher.InvokeAsync(() => {
				this.StatusLabel.Content = message;
			});
		}

		private void ToggleFaceControllerEnabledCheckBox(object sender, RoutedEventArgs e) {
			var checkBox = sender as CheckBox;
			if (checkBox != null) {
				if (checkBox.IsChecked != null) {
					ProviderManager.FaceControllerProvider.IsEnabled =
						checkBox.IsChecked.Value;
				}
			}
		}

		private void CallibrateCenterOnClick(object sender, RoutedEventArgs e) {
			ProviderManager.FaceControllerProvider.RequestCallibrateCenter();
		}
	}
}
