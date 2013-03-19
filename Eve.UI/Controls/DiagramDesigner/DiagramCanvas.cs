using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Eve.UI.Controls.DiagramDesigner {
	public class DiagramCanvas : Canvas {
		public const int DiagramItemZIndexSelected = Int32.MaxValue - 99;
		public const int DiagramItemZIndexDeselected = 99;
		private const int DiagramViewPadding = 10;

		private Point? dragStartPoint;
		private List<ISelectable> selectedItems;
		private VisualBrush diagramGridBrush;


		public DiagramCanvas() {
			this.selectedItems = new List<ISelectable>();

			// Create ParentDiagram grdi visual brush
			//<VisualBrush x:Key="DiagramDotFillBrush"
			//		TileMode="Tile"
			//		Viewport="0,0,10,10"
			//		ViewportUnits="Absolute"
			//		Viewbox="0,0,12,12"
			//		ViewboxUnits="Absolute">
			//	<VisualBrush.Visual>
			//		<Ellipse Fill="#777" Width="1" Height="1" />
			//	</VisualBrush.Visual>
			//</VisualBrush>
			var brushViewport = new Rect(0, 0, DiagramCanvas.DiagramViewPadding, DiagramCanvas.DiagramViewPadding);
			this.diagramGridBrush = new VisualBrush(
				new Ellipse() {
					Fill = new SolidColorBrush(new Color() {R = 119, G = 119, B = 119, A = 255}),
					Width = 1.0,
					Height = 1.0
				}) {
					TileMode = TileMode.Tile,
					ViewboxUnits = BrushMappingMode.Absolute,
					ViewportUnits = BrushMappingMode.Absolute,
					Viewbox = brushViewport,
					Viewport = brushViewport
				};
			this.ShowDiagramGrid = true;
			this.StickToDiagramGrid = true;
		}


		protected override Size MeasureOverride(Size constraint) {
			Size size = new Size();
			foreach (UIElement element in base.Children) {
				double left = Canvas.GetLeft(element);
				double top = Canvas.GetTop(element);
				left = double.IsNaN(left) ? 0 : left;
				top = double.IsNaN(top) ? 0 : top;

				// Measure desired size for each child
				element.Measure(constraint);

				Size desiredSize = element.DesiredSize;
				if (!double.IsNaN(desiredSize.Width) && !double.IsNaN(desiredSize.Height)) {
					size.Width = Math.Max(size.Width, left + desiredSize.Width);
					size.Height = Math.Max(size.Height, top + desiredSize.Height);
				}
			}

			// For aesthetic reasons add extra space
			size.Width += DiagramCanvas.DiagramViewPadding;
			size.Height += DiagramCanvas.DiagramViewPadding;

			return size;
		}

		protected override void OnPreviewMouseDown(System.Windows.Input.MouseButtonEventArgs e) {
			base.OnPreviewMouseDown(e);

			if (e.Source is DiagramCanvas) {
				this.DeselectAll();
				this.dragStartPoint = e.GetPosition(this);
				e.Handled = true;
			}
		}

		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);

			if (e.LeftButton != MouseButtonState.Pressed)
				this.dragStartPoint = null;

			if (this.dragStartPoint.HasValue) {
				var adornerLayer = AdornerLayer.GetAdornerLayer(this);
				if (adornerLayer != null) {
					var adorner = new SelectionBoxAdorner(this, this.dragStartPoint.Value);
					adornerLayer.Add(adorner);
				}
			}

			e.Handled = false;
		}

		protected override void OnPreviewMouseUp(MouseButtonEventArgs e) {
			base.OnPreviewMouseUp(e);

			this.dragStartPoint = null;

			e.Handled = false;
		}

		public void DeselectAll() {
			foreach (var item in this.Children.OfType<DiagramItem>()) {
				Canvas.SetZIndex(item, DiagramItemZIndexDeselected);
				item.IsSelected = false;
			}
		}

		public void SetAsSelected(ISelectable selectable, bool isSelected) {
			if (!this.selectedItems.Contains(selectable)) {
				if (isSelected) {
					this.selectedItems.Add(selectable);
					System.Diagnostics.Debug.WriteLine(String.Format("Added item to selected list {0}", selectable.GetHashCode()), typeof (DiagramCanvas).Name);
				}
			}
			else if (!isSelected) {
				this.selectedItems.Remove(selectable);
				System.Diagnostics.Debug.WriteLine(String.Format("Removed item to selected list {0}", selectable.GetHashCode()), typeof(DiagramCanvas).Name);
			}
		}

		public void ConnectionStarted(Connector source) {
			System.Diagnostics.Debug.WriteLine("Implement ConnectionStarted at DiagramCanvas!");
			foreach (var item in this.Children.OfType<DiagramItem>()) {
				item.EnableInputs(source);
			}
		}

		public void ConnectionEnded(Connector source, Connector end) {
			foreach (var item in this.Children.OfType<DiagramItem>()) {
				item.DisableInputs();
			}
		}

		#region Properties

		public bool ShowDiagramGrid {
			get { return (bool)GetValue(ShowDiagramGridProperty); }
			set { 
				SetValue(ShowDiagramGridProperty, value);

				if (value && this.diagramGridBrush != null) 
					this.Background = this.diagramGridBrush;
				else this.Background = Brushes.Transparent;
			}
		}

		public bool StickToDiagramGrid {
			get { return (bool)GetValue(StickToDiagramGridProperty); }
			set { SetValue(StickToDiagramGridProperty, value); }
		}

		public static readonly DependencyProperty StickToDiagramGridProperty =
			DependencyProperty.Register("StickToDiagramGrid", typeof(bool), typeof(DiagramCanvas), new PropertyMetadata(true));

		public static readonly DependencyProperty ShowDiagramGridProperty =
			DependencyProperty.Register("ShowDiagramGrid", typeof(bool), typeof(DiagramCanvas), new PropertyMetadata(true));

		#endregion
	}
}
