using System;
using System.Collections.Generic;
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
using Eve.Core.Loging;

namespace Eve.API.Vision {
	/// <summary>
	/// Face detection and tracking provider
	/// Provides video source too
	/// </summary>
	public class FaceDetectionProvider : IDisposable {
		// TODO Call face detection periodically
		// TODO Constants to properties
		// TODO Separate video source from detection provider
		private readonly Log.LogInstance log =
			new Log.LogInstance(typeof(FaceDetectionProvider));

		// Video Source variables
		public int VideoSourceWidth = 320;
		public int VideoSourceHeight = 240;
		public int VideoSourceStopWaitTime = 2000;
		public const int FramesToSkip = 10;
		private VideoCaptureDevice videoSource;
		private int framesSkipped;

		// Face Tracking variables
		public double DetectingFrameWidth = 160.0;
		public double DetectingFrameHeight = 120.0;
		public float DetectingAreaStep = 1.2f;
		public int DetectingAreaMin = 25;
		private HaarObjectDetector detector;
		private List<FaceTracker> trackers;
		private bool isDetectingInProgress;


		/// <summary>
		/// Object constructor
		/// </summary>
		public FaceDetectionProvider() {
			this.log.Info("Object instance created");

			this.Reset();
		}

		/// <summary>
		/// Object destructor
		/// </summary>
		~FaceDetectionProvider() {
			this.log.Info("Object instance destroyed");
		}


		/// <summary>
		/// Initializes video source and detection components
		/// </summary>
		/// <returns>Returns asynchronous void Task</returns>
		public async Task InitializeAsync() {
			this.log.Info("Initialization started...");
			var sw = new System.Diagnostics.Stopwatch();
			sw.Start();

			// Clear trackers list
			this.trackers.Clear();

			// Initialize components
			await this.InitializeVideoSourceAsync();
			await this.InitializeDetectorsAsync();

			sw.Stop();
			this.log.Info("Initialization successful in {0}ms", sw.ElapsedMilliseconds);
		}

		/// <summary>
		/// Looks for available video sources 
		/// </summary>
		/// <returns>Returns asynchronous void Task</returns>
		private async Task InitializeVideoSourceAsync(string monkierString = null) {
			// Get available video sources
			var availableVideoSources =
				new FilterInfoCollection(FilterCategory.VideoInputDevice);
			if (availableVideoSources.Count <= 0)
				throw new Exception("No video devices available");

			// Create video source object from first found or of given name
			try {
				this.videoSource =
					new VideoCaptureDevice(monkierString ??
										   availableVideoSources[0].MonikerString) {
											   DesiredFrameSize =
												   new Size(this.VideoSourceWidth, this.VideoSourceHeight)
										   };
			}
			catch (Exception ex) {
				this.log.Error<Exception>(ex, "Couldn't create video capture device");
				return;
			}

			// Wait for video source to stop
			await this.StopVideoSource();
		}

		/// <summary>
		/// Stops video source signal if already not stopped;
		/// </summary>
		/// <returns>Returns asynchronous void Task</returns>
		private async Task StopVideoSource() {
			if (this.videoSource == null) {
				throw new NullReferenceException(
					"Can't stop video source if null. videoSource is null");
			}

			// Signal camera to stop recording
			this.videoSource.SignalToStop();

			// Wait for max. 2 seconds for camera to stop recording
			await Task.Run(() => {
				const int waitTime = 20;
				for (int time = 0;
					 time < this.VideoSourceStopWaitTime && this.videoSource.IsRunning;
					 time += waitTime) {
					System.Threading.Thread.Sleep(waitTime);
				}
			});

			// Force camera to stop if still running
			if (this.videoSource.IsRunning)
				this.videoSource.Stop();
		}

		/// <summary>
		/// Creates Face Haar detector
		/// </summary>
		/// <returns>Returns asynchronous void Task</returns>
		private async Task InitializeDetectorsAsync() {
			await Task.Run(() => {
				this.log.Info("Initializing face detector...");

				// InitializeAsync face detector for multiple objects
				this.detector = new HaarObjectDetector(
					new FaceHaarCascade(),
					this.DetectingAreaMin, ObjectDetectorSearchMode.NoOverlap,
					this.DetectingAreaStep, ObjectDetectorScalingMode.GreaterToSmaller) {
						UseParallelProcessing = true
					};
			});
		}

		/// <summary>
		/// Processes given frame - detecting faces and tracking objects
		/// </summary>
		/// <param name="frame">Frame to process</param>
		/// <returns>Returns asynchronous void Task</returns>
		public async Task ProcessFrameAsync(UnmanagedImage frame) {
			// Skip first few frames during initialization
			if (this.framesSkipped <= FramesToSkip) {
				this.framesSkipped++;
				return;
			}

			// Initiate face detection if no face is tracking
			if (!isDetectingInProgress && !this.trackers.Any(t => t.IsActive))
				this.DetectFacesAsync(frame);

			// Track all active faces
			this.TrackFaces(frame);
		}

		/// <summary>
		/// Detects face regions and creates trackers
		/// </summary>
		/// <param name="image">Image to search for faces on</param>
		private async void DetectFacesAsync(UnmanagedImage image) {
			var sw = new System.Diagnostics.Stopwatch();
			sw.Start();
			this.isDetectingInProgress = true;

			double xScale = image.Width / this.DetectingFrameWidth;
			double yScale = image.Height / this.DetectingFrameHeight;

			// Resize image (downsample)
			var resize = new ResizeNearestNeighbor((int) this.DetectingFrameWidth,
												   (int) this.DetectingFrameHeight);
			UnmanagedImage downsampledImage = resize.Apply(image);

			// Get detector regions
			var regions = new Rectangle[0];
			await Task.Run(() => regions = detector.ProcessFrame(downsampledImage));

			// Check if the face has been steady for 5 frames in a row
			if (this.detector.Steady > 5) {
				// Create face tracker for each of detected regions
				foreach (var region in regions) {
					this.CreateFaceTracker(region, xScale, yScale, image);
				}
			}

			this.isDetectingInProgress = false;
			sw.Stop();
			this.log.Info("\n{0} faces detected in {1}ms",
						  regions.Length, sw.ElapsedMilliseconds);
		}

		/// <summary>
		/// Creates new face tracker
		/// Recycles inactive trackers
		/// </summary>
		/// <param name="trackingRegion">Region of image to track</param>
		/// <param name="xScale">X scale of downsampled image to original image</param>
		/// <param name="yScale">Y scale if downsampled image to original image</param>
		/// <param name="image">Downsampled image from which we trace given rectangle</param>
		private void CreateFaceTracker(Rectangle trackingRegion, double xScale,
									   double yScale, UnmanagedImage image) {
			// Find first not active tracker 
			// object or create new one if none found
			int faceTrackerIndex = -1;
			for (int index = 0; index < this.trackers.Count; index++) {
				if (!this.trackers.ElementAt(index).IsActive) {
					faceTrackerIndex = index;
					break;
				}
			}
			FaceTracker faceTracker;
			if (faceTrackerIndex < 0) {
				faceTracker = new FaceTracker();
				this.trackers.Add(faceTracker);
			}
			else faceTracker = this.trackers.ElementAt(faceTrackerIndex);

			// Reduce the face size to avoid tracking background
			var trackingArea = new Rectangle(
				(int)((trackingRegion.X + trackingRegion.Width / 2f) * xScale),
				(int)((trackingRegion.Y + trackingRegion.Height / 2f) * yScale),
				1, 1);
			trackingArea.Inflate(
				(int)(0.25f * trackingRegion.Width * xScale),
				(int)(0.40f * trackingRegion.Height * yScale));

			// Apply tracking area to tracker object
			faceTracker.IsActive = true;
			faceTracker.Tracker.Reset();
			faceTracker.Tracker.SearchWindow = trackingArea;
			faceTracker.Tracker.ProcessFrame(image);
		}

		/// <summary>
		/// Processes all available trackers for face on given frame
		/// </summary>
		/// <param name="image">Frame to process for trackers</param>
		private void TrackFaces(UnmanagedImage image) {
			// Do tracking for all trackers
			Parallel.ForEach(this.trackers, async (tracker) => {
				await this.TrackFaceAsync(tracker, image);
			});
		}

		/// <summary>
		/// Processes given frame for given face tracker
		/// </summary>
		/// <param name="faceTracker">Face tracker to process</param>
		/// <param name="image">Frame to process for tracker</param>
		/// <returns>Returns asynchronous void Task</returns>
		private async Task TrackFaceAsync(FaceTracker faceTracker, UnmanagedImage image) {
			// Check if face tracker is active
			if (!faceTracker.IsActive) return;

			// Process given frame
			await Task.Run(() => faceTracker.Tracker.ProcessFrame(image));

			// Get object properties
			var trackingObject = faceTracker.Tracker.TrackingObject;

			// If tracking object is empty, start new detection
			if (trackingObject.IsEmpty || trackingObject.Area < 5) {
				faceTracker.IsActive = false;
				this.log.Info("Face not in view, tracker set to inactive state");
				return;
			}
		}

		private void Reset() {
			// Set values to default
			if (this.trackers != null)
				this.trackers.ForEach(t => t.IsActive = false);
			else this.trackers = new List<FaceTracker>();
			this.framesSkipped = 0;
		}


		#region Properties

		/// <summary>
		/// Gets video source of provider
		/// </summary>
		public IVideoSource Source {
			get { return this.videoSource; }
		}

		/// <summary>
		/// Gets all trackers active and inactive
		/// </summary>
		public IEnumerable<FaceTracker> Trackers {
			get { return this.trackers; }
		}

		#endregion

		/// <summary>
		/// Disposes (stops) video source if available
		/// </summary>
		public async void Dispose() {
			if (this.videoSource != null)
				await this.StopVideoSource();
		}

		/// <summary>
		/// FaceTracker class used for tracking one face object
		/// Contains marker for easy development
		/// </summary>
		public class FaceTracker {
			public bool IsActive { get; set; }
			public Camshift Tracker { get; private set; }
			public RectanglesMarker Marker { get; private set; }


			/// <summary>
			/// Object constructor
			/// Created RGB tracker and empty rectangle marker
			/// </summary>
			public FaceTracker() {
				// Create tracker
				this.Tracker = new Camshift() {
					Mode = CamshiftMode.RGB,
					Conservative = true,
					AspectRatio = 1.5f
				};

				// Create marker
				this.Marker = new RectanglesMarker();
			}


			/// <summary>
			/// Creates new rectangle of current tracker search window
			/// </summary>
			private void UpdateMarkers() {
				this.Marker = new RectanglesMarker(this.Tracker.SearchWindow);
			}

			/// <summary>
			/// Draws rectangle around tracking object
			/// </summary>
			/// <param name="image">Image to add markers to</param>
			/// <param name="drawAxis">Whether to draw X axis line</param>
			public void DrawMarker(ref UnmanagedImage image, bool drawAxis = false) {
				// Update marker
				this.UpdateMarkers();

				// Draws X and Y axis of object
				if (drawAxis) {
					LineSegment axis = this.Tracker.TrackingObject.GetAxis();
					if (axis != null)
						Drawing.Line(image, axis.Start.Round(), axis.End.Round(), Color.Red);
				}

				// Draw market on image
				if (this.Marker != null)
					this.Marker.ApplyInPlace(image);
			}
		}
	}
}
