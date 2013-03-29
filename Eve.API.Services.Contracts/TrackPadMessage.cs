using System;
using System.Runtime.Serialization;

namespace Eve.API.Services.Contracts {
	[DataContract(Name = "TrackPadMessage")]
	public class TrackPadMessage {
		public TrackPadMessage(TrackPadCommands command,
		                       double x = default(double), double y = default(double),
		                       Orientation direction = default(Orientation),
		                       double angle = default(double), double ratio = default(double)) {
			this.Command = command;
			this.X = x;
			this.Y = y;
			this.Direction = direction;
			this.Angle = angle;
			this.DistanceRatio = ratio;
		}

		public override string ToString() {
			return String.Format("Command: {0}  |  (X,Y): ({1},{2})  |  Direction: {3}  |  Angle: {4}  |  Ratio: {5}",
			                     this.Command.ToString(), this.X, this.Y, this.Direction.ToString(), this.Angle,
			                     this.DistanceRatio);
		}

		#region Properties

		[DataMember]
		public TrackPadCommands Command { get; set; }

		/// <summary>
		/// Gets and sets X axis of command
		/// HorizontalChange for Drag
		/// HorizontalVelocity for Flick
		/// </summary>
		[DataMember]
		public double X { get; set; }

		/// <summary>
		/// Gets and sets Y axis of command
		/// VerticalChange for Drag
		/// VerticalVelocity for Flick
		/// </summary>
		[DataMember]
		public double Y { get; set; }

		/// <summary>
		/// Gets and sets direction of command
		/// </summary>
		[DataMember]
		public Orientation Direction { get; set; }

		/// <summary>
		/// Gets or sets angle of command
		/// Angle for Flick
		/// TotalAngleDelta for Pinch
		/// Angle for PinchStarted
		/// </summary>
		[DataMember]
		public double Angle { get; set; }

		/// <summary>
		/// Gets and sets distance ratio for command
		/// DistanceRatio for Pinch
		/// Distance for PichStarted
		/// </summary>
		[DataMember]
		public double DistanceRatio { get; set; }

		#endregion

		public enum TrackPadCommands {
			Tap,
			Hold,
			DoubleTap,
			Flick,
			DragStarted,
			DragDelta,
			DragCompleted,
			PinchStarted,
			PinchDelta,
			PinchCompleted
		}
	}
}