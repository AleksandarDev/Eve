using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Eve.UI.Controls.DiagramDesigner {
	public class SelectionBoxAdorner : Adorner {
		private readonly DiagramCanvas diagram;
		private readonly Pen selectionBoxPen;
		private Point? startPoint, endPoint;


		public SelectionBoxAdorner(DiagramCanvas parent, Point startPoint) : base(parent) {
			this.diagram = parent;
			this.startPoint = startPoint;

			this.selectionBoxPen = new Pen(new SolidColorBrush(new Color() {R = 0, G = 122, B = 204, A = 50}), 1);
		}


		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);

			if (e.LeftButton == MouseButtonState.Pressed) {
				if (!this.IsMouseCaptured)
					this.CaptureMouse();

				this.endPoint = e.GetPosition(this);
				this.Update();
				this.InvalidateVisual();
			}
			else if (this.IsMouseCaptured) this.ReleaseMouseCapture();

			e.Handled = true;
		}

		protected override void OnPreviewMouseUp(MouseButtonEventArgs e) {
			base.OnPreviewMouseUp(e);

			// Release mouse capture
			if (this.IsMouseCaptured)
				this.ReleaseMouseCapture();

			// Remove adorner from adorner layer
			var adornerLayer = this.Parent as AdornerLayer;
			if (adornerLayer != null)
				adornerLayer.Remove(this);

			e.Handled = true;
		}

		protected override void OnRender(DrawingContext dc) {
			base.OnRender(dc);

			// Without a background the OnMouseMove event would not be fired!
			// Alternative: implement a Canvas as a child of this adorner, like
			// the ConnectionAdorner does.
			dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));

			if (this.startPoint.HasValue && this.endPoint.HasValue)
				dc.DrawRectangle(new SolidColorBrush(new Color() {R = 0, G = 122, B = 204, A = 25}), selectionBoxPen,
				                 new Rect(this.startPoint.Value, this.endPoint.Value));
		}

		private void Update() {
			if (this.startPoint.HasValue && this.endPoint.HasValue) {
				// Update DiagramItem selection
				Rect rubberBand = new Rect(this.startPoint.Value, this.endPoint.Value);
				foreach (var item in this.diagram.Children.OfType<DiagramItem>()) {
					Rect itemRect = VisualTreeHelper.GetDescendantBounds(item);
					Rect itemBounds = item.TransformToAncestor(diagram).TransformBounds(itemRect);

					item.IsSelected = rubberBand.Contains(itemBounds);
				}
			}
		}
	}
}