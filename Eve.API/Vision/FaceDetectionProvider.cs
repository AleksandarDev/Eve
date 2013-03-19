using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;
using AForge.Video;
using AForge.Video.DirectShow;
using Accord.Imaging.Filters;
using Accord.Vision.Detection;
using Accord.Vision.Detection.Cascades;
using Accord.Vision.Tracking;

namespace Eve.API.Vision {
	public class FaceDetectionProvider : IDisposable {
		// Video Source variables
		public int VideoSourceWidth = 320;
		public int VideoSourceHeight = 240;
		public int VideoSourceStopWaitTime = 2000;
		private VideoCaptureDevice videoSource;

		// Face Tracking variables
		public double DetectingFrameWidth = 320.0;
		public double DetectingFrameHeight = 240.0;
		public float DetectingAreaStep = 1.2f;
		public int DetectingAreaMin = 35;
		private bool isTracking;
		private HaarObjectDetector detector;
		private Camshift tracker;
		private RectanglesMarker marker;


		public FaceDetectionProvider() {

		}


		public async Task Initialize() {
			await this.InitializeVideoSource();
			await this.InitializeDetectors();		
		}

		private async Task InitializeVideoSource() {
			// Get available video sources
			var availableVideoSources = new FilterInfoCollection(FilterCategory.VideoInputDevice);
			if (availableVideoSources.Count <= 0)
				throw new Exception("No video devices available");

			// Create video source object
			this.videoSource = new VideoCaptureDevice(availableVideoSources[0].MonikerString) {
				DesiredFrameSize = new Size(this.VideoSourceWidth, this.VideoSourceHeight)
			};

			// Wait for video source to stop
			await this.StopVideoSource();
		}

		private async Task InitializeDetectors() {
			await Task.Run(() => {
				// Initialize detector
				this.detector = new HaarObjectDetector(
					new FaceHaarCascade(),
					this.DetectingAreaMin, ObjectDetectorSearchMode.Single,
					this.DetectingAreaStep, ObjectDetectorScalingMode.GreaterToSmaller) {
						UseParallelProcessing = true
					};

				// Initialize tracker
				this.tracker = new Camshift() {
					Mode = CamshiftMode.RGB,
					Conservative = false,
					AspectRatio = 1.5f
				};
			});
		}

		private async Task StopVideoSource() {
			// Signal camera to stop recording
			this.videoSource.SignalToStop();

			// Wait for max. 2 seconds for camera to stop recording
			await Task.Run(() => {
				const int waitTime = 20;
				for (int time = 0; time < this.VideoSourceStopWaitTime && this.videoSource.IsRunning; time += waitTime) {
					System.Threading.Thread.Sleep(waitTime);
				}
			});

			// Force camera to stop if still runing
			if (this.videoSource.IsRunning)
				this.videoSource.Stop();
		}

		public void ProcessFrame(ref Bitmap frame) {
			if (this.isTracking)
				this.TrackObjects(ref frame);
			else this.DetectFaces(ref frame);
		}

		private void DetectFaces(ref Bitmap image) {
			Stopwatch sw = new Stopwatch();
			sw.Start();

			// Get unmanaged image from given managed image
			UnmanagedImage unmanagedImage = UnmanagedImage.FromManagedImage(image);

			double xScale = image.Width/this.DetectingFrameWidth;
			double yScale = image.Height/this.DetectingFrameHeight;

			// Resize image (downsample)
			ResizeNearestNeighbor resize = new ResizeNearestNeighbor((int)this.DetectingFrameWidth, (int)this.DetectingFrameHeight);
			UnmanagedImage downsampledImage = resize.Apply(unmanagedImage);

			// Get detector regions
			Rectangle[] regions = detector.ProcessFrame(downsampledImage);

			// If no regions were found, countinue detecting...
			if (regions.Length <= 0) {
				this.isTracking = false;
				return;
			}
			
			// Initiate tracking
			this.isTracking = true;
			this.tracker.Reset();

			// Select first available tracking region
			Rectangle trackingRegion = regions.FirstOrDefault();

			// Reduce the face size to avoid tracking background
			Rectangle trackingArea = new Rectangle(
				(int) ((trackingRegion.X + trackingRegion.Width/2f)*xScale),
				(int) ((trackingRegion.Y + trackingRegion.Height/2f)*yScale),
				1, 1);
			trackingArea.Inflate(
				(int) (0.2f*regions[0].Width*xScale),
				(int) (0.4f*regions[0].Height*yScale));

			// Apply tracking are to tracker object
			this.tracker.SearchWindow = trackingArea;
			this.tracker.ProcessFrame(unmanagedImage);

			// Create marker
			this.marker = new RectanglesMarker(trackingArea);
			this.marker.ApplyInPlace(unmanagedImage);

			// Set unmanaged image as managed image 
			image = unmanagedImage.ToManagedImage();

			sw.Stop();
			if (this.isTracking)
				System.Diagnostics.Debug.WriteLine(String.Format("\nFace detected in {0}ms", sw.ElapsedMilliseconds));
		}

		private void TrackObjects(ref Bitmap image) {
			// Get unmanaged image from given managed image
			UnmanagedImage unmanagedImage = UnmanagedImage.FromManagedImage(image);

			// Process given frame
			this.tracker.ProcessFrame(unmanagedImage);

			// Get object properties
			var trackingObject = this.tracker.TrackingObject;
			var trackingArea = this.tracker.SearchWindow;

			// If tracking object is empty, start new detection
			if (trackingObject.IsEmpty) {
				this.isTracking = false;
				System.Diagnostics.Debug.WriteLine("Looking for faces...");
				return;
			}

			// Draws X and Y axis of object
			LineSegment axis = trackingObject.GetAxis();
			Drawing.Line(unmanagedImage, axis.Start.Round(), axis.End.Round(), Color.Red);

			// Apply marker
			this.marker = new RectanglesMarker(trackingArea);

			// Draw market on image and set unmanaged image as managed image
			if (this.marker != null)
				this.marker.ApplyInPlace(unmanagedImage);
			image = unmanagedImage.ToManagedImage();
		}


		#region Properties

		public IVideoSource Source {
			get { return this.videoSource; }
		}

		#endregion

		public async void Dispose() {
			await this.StopVideoSource();
		}
	}
}
