using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Eve.UI.Controls.DiagramDesigner {
	public class ConnectorAdorner : Adorner {
		private readonly Connector connector;
		private readonly Pen connectorPen;
		private PathGeometry pathGeometry;


		public ConnectorAdorner(Connector connector) : base(connector.ParentDiagramItem.ParentDiagram) {
			this.connector = connector;

			this.connectorPen = new Pen(Brushes.LightGray, 2) {LineJoin = PenLineJoin.Round};
			this.Cursor = Cursors.Cross;

			System.Diagnostics.Debug.WriteLine("Connector selected", typeof (ConnectorAdorner).Name);
		}


		protected override void OnPreviewMouseUp(MouseButtonEventArgs e) {
			base.OnPreviewMouseUp(e);

			if (this.HitConnector != null) {
				System.Diagnostics.Debug.WriteLine("Connection made");
			}

			if (this.HitDiagramItem != null) {
				// TODO Implement
				//this.HitDiagramItem.IsDragConnectionOver = false;
				
			}

			if (this.IsMouseCaptured)
				this.ReleaseMouseCapture();

			var adornerLayer = AdornerLayer.GetAdornerLayer(this.connector.ParentDiagramItem.ParentDiagram);
			if (adornerLayer != null) {
				adornerLayer.Remove(this);
				//this.connector.ParentDiagramItem.IsConnecting = false;
				System.Diagnostics.Debug.WriteLine("Connector released", typeof (ConnectorAdorner).Name);
			}
		}

		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);

			if (e.LeftButton == MouseButtonState.Pressed) {
				if (!this.IsMouseCaptured)
					this.CaptureMouse();

				var mousePosition = e.GetPosition(this);
				this.HitTesting(mousePosition);
				this.pathGeometry = GetPathGeometry(mousePosition);

				this.InvalidateVisual();
			}
			else if (this.IsMouseCaptured)
				this.ReleaseMouseCapture();
		}

		private PathGeometry GetPathGeometry(Point mousePosition) {
			var geometry = new PathGeometry();
			var figure = new PathFigure();
			figure.StartPoint = this.connector.Position;
			figure.Segments.Add(new PolyLineSegment(new[] {mousePosition}, true));
			geometry.Figures.Add(figure);
			return geometry;
		}

		private void HitTesting(Point testPoint) {
			bool hitConnectorFlag = false;

			DependencyObject hitObject = this.connector.ParentDiagramItem.ParentDiagram.InputHitTest(testPoint) as DependencyObject;
			while (hitObject != null &&
			       hitObject != this.connector.ParentDiagramItem &&
			       hitObject.GetType() != typeof (DiagramCanvas)) {
				if (hitObject is Connector) {
					this.HitConnector = hitObject as Connector;
					hitConnectorFlag = true;
				}

				if (hitObject is DiagramItem) {
					HitDiagramItem = hitObject as DiagramItem;
					if (!hitConnectorFlag)
						HitConnector = null;
					return;
				}

				hitObject = VisualTreeHelper.GetParent(hitObject);
			}
		}

		protected override void OnRender(DrawingContext dc) {
			base.OnRender(dc);
			dc.DrawGeometry(null, this.connectorPen, this.pathGeometry);

			// Without a background the OnMouseMove event would not be fired
			// Alternative: implement a Canvas as a child of this adorner, like
			// the ConnectionAdorner does.
			dc.DrawRectangle(Brushes.Transparent, null, new Rect(this.RenderSize));
		}


		#region Properties

		//private DiagramItem hitDiagramItem;
		private DiagramItem HitDiagramItem { get; set; }
		//private DiagramItem HitDiagramItem {
		//	get { return this.hitDiagramItem; }
		//	set {
		//		if (this.hitDiagramItem != value) {
		//			//if (this.hitDiagramItem != null)
		//			//	this.hitDiagramItem.IsDragConnectionOver = false;

		//			this.hitDiagramItem = value;

		//			//if (this.hitDiagramItem != null)
		//			//	this.hitDiagramItem.OsDragConnectionOver = true;
		//		}
		//	}
		//}

		//private Connector hitConnector;
		private Connector HitConnector { get; set; }

		#endregion
	}
}