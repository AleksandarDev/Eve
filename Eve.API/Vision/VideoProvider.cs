using System;
using System.Threading.Tasks;
using Accord.Video;
using Accord.Video.DirectShow;

namespace Eve.API.Vision {
	[ProviderDescription("Video Provider")]
	public class VideoProvider : ProviderBase<VideoProvider> {
		// Default settings
		private const int DefaultSourceWidth = 320;
		private const int DefaultSourceHeight = 240;
		private const int DefaultStopWaitTime = 2000;
		private const int DefaultStopCheckPeriod = 20;

		protected override void Initialize() {
			this.SourceWidth = DefaultSourceWidth;
			this.SourceHeight = DefaultSourceHeight;
			this.StopWaitTime = DefaultStopWaitTime;
			this.StopCheckPeriod = DefaultStopCheckPeriod;

			this.StartDevice();
		}

		protected override void Uninitialize() {
			this.StopDevice();
		}

		private void StartDevice() {
			this.log.Info("Starting video device...");

			// Get available video sources
			var availableVideoSources =
				new FilterInfoCollection(FilterCategory.VideoInputDevice);
			if (availableVideoSources.Count <= 0)
				throw new Exception("No video devices available");

			// Create video source object
			this.Device = new VideoCaptureDevice(
				availableVideoSources[0].MonikerString);

			// Wait for video source to stop if already running
			this.StopDevice();

			// Start video device
			this.Device.Start();

			this.log.Info("Video device started");
		}

		private void StopDevice() {
			this.log.Info("Stopping video device...");

			// Signal camera to stop recording
			this.Device.SignalToStop(); 

			// Wait for some amount of seconds for camera to stop recording
			for (int time = 0; time < this.StopWaitTime && this.Device.IsRunning; time += this.StopCheckPeriod) 
				System.Threading.Thread.Sleep(this.StopCheckPeriod);

			// Force camera to stop if still running
			if (this.Device.IsRunning)
				this.Device.Stop();

			this.log.Info("Video device stopped");
		}

		public async Task<bool> ChangeDeviceAsync(string deviceMoniker) {
			throw new NotImplementedException();
		}

		#region Properties

		public int SourceWidth { get; set; }
		public int SourceHeight { get; set; }
		public int StopWaitTime { get; set; }
		public int StopCheckPeriod { get; set; }
		public VideoCaptureDevice Device { get; private set; }
		public IVideoSource DeviceVideoSource { get { return this.Device as IVideoSource; } }

		#endregion
	}
}