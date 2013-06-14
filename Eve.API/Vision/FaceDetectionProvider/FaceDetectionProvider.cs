using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;
using AForge.Video;
using Accord.Imaging.Filters;
using Accord.Vision.Detection;
using Accord.Vision.Detection.Cascades;
using Accord.Vision.Tracking;
using Eve.Diagnostics.Logging;

namespace Eve.API.Vision {
	[ProviderDescription("Face Detection Provider", typeof(VideoProvider))]
	public class FaceDetectionProvider : ProviderBase<FaceDetectionProvider> {
		// TODO Call face detection periodically
		private const int FramesToSkip = 10;
		private int framesSkipped = 0;
		private HaarObjectDetector detector;
		private List<FaceTracker> trackers;
		private bool isDetectingInProgress;

		public double DetectingFrameWidth = 160.0;
		public double DetectingFrameHeight = 120.0;
		public float DetectingAreaStep = 1.2f;
		public int DetectingAreaMin = 25;

		public event FaceDetectionEventHandler OnFaceDetected;
		public event FaceDetectionEventHandler OnFaceLost;
		public event FaceDetectionEventHandler OnFaceMove;


		protected override void Initialize() {
			// Clear trackers list
			this.trackers = new List<FaceTracker>();

			// Initialize components
			this.InitializeDetectors();

			ProviderManager.VideoProvider.Device.NewFrame += this.ProcessFrame;
		}

		protected override void Uninitialize() {
			ProviderManager.VideoProvider.Device.NewFrame -= this.ProcessFrame;

			this.detector = null;

			if (this.trackers != null) {
				this.trackers.Clear();
				this.trackers = null;
			}
		}

		private void InitializeDetectors() {
			this.log.Info("Initializing face detector...");

			// InitializeAsync face detector for multiple objects
			this.detector = new HaarObjectDetector(
				new FaceHaarCascade(),
				this.DetectingAreaMin, ObjectDetectorSearchMode.NoOverlap,
				this.DetectingAreaStep, ObjectDetectorScalingMode.GreaterToSmaller) {
					UseParallelProcessing = true
				};
		}

		private void ProcessFrame(Object sender, NewFrameEventArgs args) {
			if (!this.CheckIsRunning()) return;

			var unmanagedImage = UnmanagedImage.FromManagedImage(args.Frame);
			this.ProcessFrame(ref unmanagedImage);
			unmanagedImage.Dispose();
		}

		public void ProcessFrame(ref UnmanagedImage frame) {
			if (!this.CheckIsRunning()) return;

			// Skip first few frames during initialization
			if (this.framesSkipped <= FramesToSkip) {
				this.framesSkipped++;
				return;
			}

			// Initiate face detection if no face is tracking
			if (!this.isDetectingInProgress && !this.trackers.Any(t => t.IsActive))
				this.DetectFaces(ref frame);
			else this.TrackFaces(ref frame); // Track all active faces
		}

		private void DetectFaces(ref UnmanagedImage image) {
			if (!this.CheckIsRunning()) return;

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
			var regions = this.detector.ProcessFrame(downsampledImage);

			// Check if the face has been steady for 5 frames in a row
			if (this.detector != null && this.detector.Steady > 5) {
				// Create face tracker for each of detected regions
				for (int index = 0; regions != null && index < regions.Length; index++) 
					this.CreateFaceTracker(regions.ElementAt(index), xScale, yScale, ref image);
			}

			this.isDetectingInProgress = false;
			sw.Stop();
			this.log.Info("\n{0} faces detected in {1}ms",
						  regions.Length, sw.ElapsedMilliseconds);
		}

		private void CreateFaceTracker(Rectangle trackingRegion, double xScale,
									   double yScale, ref UnmanagedImage image) {
			if (!this.CheckIsRunning()) return;

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

			if (this.OnFaceDetected != null)
				this.OnFaceDetected(this,
					new FaceDetectionEventArgs() {Tracker = faceTracker});
		}

		private void TrackFaces(ref UnmanagedImage image) {
			if (!this.CheckIsRunning()) return;

			// Do tracking for all trackers
			for (int index = 0; this.trackers != null && index < this.trackers.Count; index++)
				this.TrackFace(this.trackers.ElementAt(index), ref image);

			// Remove not active face trackers
			if (this.trackers != null) {
				var nonActive = this.trackers.Where(t => !t.IsActive).ToList();
				for (int index = 0; index < nonActive.Count; index++) 
					this.trackers.Remove(nonActive.ElementAt(index));
			}
		}

		private void TrackFace(FaceTracker faceTracker, ref UnmanagedImage image) {
			if (!this.CheckIsRunning()) return;

			// Check if face tracker is active
			if (!faceTracker.IsActive) return;

			// Save tracking object state before updating it so that
			// we can determine if any change has been made to the tracker
			var oldObject = faceTracker.Tracker.TrackingObject.Clone();

			// Process given frame
			faceTracker.Tracker.ProcessFrame(image);

			// Get object properties
			var trackingObject = faceTracker.Tracker.TrackingObject;

			// If tracking object is empty, start new detection
			if (trackingObject.IsEmpty || trackingObject.Area < 5) {
				faceTracker.IsActive = false;

				this.log.Info("Face not in view, tracker set to inactive state");

				if (this.OnFaceLost != null)
					this.OnFaceLost(this, new FaceDetectionEventArgs() {Tracker = faceTracker});

				return;
			}

			// Check if we need to call event and if any change
			// to tracker has been made during processing
			if (this.OnFaceMove != null &&
				oldObject != faceTracker.Tracker.TrackingObject)
				this.OnFaceMove(this, new FaceDetectionEventArgs() {Tracker = faceTracker});
		}


		#region Properties

		public IEnumerable<FaceTracker> Trackers {
			get { return this.trackers; }
		}

		#endregion

		public class FaceTracker {
			public bool IsActive { get; set; }
			public Camshift Tracker { get; private set; }
			public RectanglesMarker Marker { get; private set; }


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


			private void UpdateMarkers() {
				this.Marker = new RectanglesMarker(this.Tracker.SearchWindow);
			}

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
