using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Eve.UI.Controls.DiagramDesigner {
	public class DragThumb : Thumb {
		public DragThumb() {
			this.DragDelta += DragDeltaHandle;
		}

		private void DragDeltaHandle(object sender, DragDeltaEventArgs e) {
			var control = this.TemplatedParent as DiagramItem;
			if (control != null) {
				var diagram = VisualTreeHelper.GetParent(control) as DiagramCanvas;
				if (diagram != null) {
					double hDelta = control.ParentDiagram.StickToDiagramGrid ? 10 * Math.Ceiling(e.HorizontalChange / 10) : e.HorizontalChange;
					double vDelta = control.ParentDiagram.StickToDiagramGrid ? 10 * Math.Ceiling(e.VerticalChange / 10) : e.VerticalChange;

					bool isHorizontalMove = !control.ParentDiagram.StickToDiagramGrid || Math.Abs(e.HorizontalChange) > 5.0;
					bool isVerticalMove = !control.ParentDiagram.StickToDiagramGrid || Math.Abs(e.VerticalChange) > 5.0;

					foreach (var item in diagram.Children.OfType<DiagramItem>()) {
						if (!item.IsSelected) continue;

						double left = Canvas.GetLeft(item);
						double top = Canvas.GetTop(item);

						if (isHorizontalMove && left + hDelta > 0)
							Canvas.SetLeft(item, left + hDelta);
						if (isVerticalMove && top + vDelta > 0)
							Canvas.SetTop(item, top + vDelta);	
					}

					diagram.InvalidateMeasure();
				}
			}
		}
	}
}
