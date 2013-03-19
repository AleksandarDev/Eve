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

namespace Eve.UI.Controls.DiagramDesigner {
	public class Connector : Control {
		private Point? startDragPoint;
		private DiagramItem diagramItem;


		public Connector() : base() {
			this.LayoutUpdated += ConnectorLayoutUpdated;
		}

		private void ConnectorLayoutUpdated(object sender, EventArgs e) {
			this.diagramItem = this.GetDiagramItem();
			if (this.diagramItem == null)
				throw new NullReferenceException("Can't find DiagramItem parent");

			if (this.ParentDiagramItem.ParentDiagram != null) {
				this.Position = this.TransformToAncestor(this.ParentDiagramItem.ParentDiagram)
									.Transform(new Point(this.Width / 2, this.Height / 2));
			}
		}

		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
			base.OnMouseLeftButtonDown(e);
			
			// Set start drag position on left click if diagram is available
			if (this.ParentDiagramItem.ParentDiagram != null) {
				this.startDragPoint = e.GetPosition(this.ParentDiagramItem.ParentDiagram);
				e.Handled = true;
			}
		}

		protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e) {
			base.OnMouseMove(e);
			System.Diagnostics.Debug.WriteLine(this.startDragPoint.HasValue ? this.startDragPoint.Value.ToString() : "null");
			// If mouse button is not pressed we don't do drag operation
			if (e.LeftButton != MouseButtonState.Pressed)
				this.startDragPoint = null;
			if (!this.startDragPoint.HasValue) {
				System.Diagnostics.Debug.WriteLine("No start point on connector");
				return;
			}

			// If start drag position is set and mouse button is pressed
			// and diagram isn't null
			if (this.ParentDiagramItem.ParentDiagram == null) {
				System.Diagnostics.Debug.WriteLine("Can't get diagram is null!", typeof(Connector).Name);
				return;
			}

			// Get diagram adorner layer
			var adornerLayer = AdornerLayer.GetAdornerLayer(this.ParentDiagramItem.ParentDiagram);
			if (adornerLayer == null) {
				System.Diagnostics.Debug.WriteLine("Can't get diagram adorner layer!", typeof (Connector).Name);
				return;
			}
			
			// Create adorner and add it to diagram
			this.Adorner = new ConnectorAdorner(this);
			adornerLayer.Add(Adorner);
			
			// Set needed values so that diagram can correcly show other connectors
			this.ParentDiagramItem.ParentDiagram.ConnectionStarted(this);

			e.Handled = true;
		}

		private DiagramItem GetDiagramItem() {
			var container = this.Parent as Grid;
			if (container != null)
				return container.Parent as DiagramItem;
			return null;
		}

		#region Properties

		public DiagramItem ParentDiagramItem { get { return this.diagramItem; } }

		public ConnectorAdorner Adorner { get; private set; }

		public Point Position {
			get { return (Point)GetValue(PositionProperty); }
			set { SetValue(PositionProperty, value); }
		}

		public Brush Fill {
			get { return (Brush)GetValue(FillProperty); }
			set { SetValue(FillProperty, value); }
		}

		public double Size {
			get { return (double)GetValue(SizeProperty); }
			set { SetValue(SizeProperty, value); }
		}

		public Types Type {
			get { return (Types)GetValue(TypeProperty); }
			set { SetValue(TypeProperty, value); }
		}

		public States State {
			get { return (States)GetValue(StateProperty); }
			set { SetValue(StateProperty, value); }
		}

		public static readonly DependencyProperty StateProperty =
			DependencyProperty.Register("State", typeof(States), typeof(Connector), new PropertyMetadata(States.ZState));

		public static readonly DependencyProperty TypeProperty =
			DependencyProperty.Register("Type", typeof(Types), typeof(Connector), new PropertyMetadata(Types.Boolean));

		public static readonly DependencyProperty IsConnectionOverProperty =
			DependencyProperty.Register("IsConnectionOver", typeof(bool), typeof(Connector), new PropertyMetadata(false));

		public static readonly DependencyProperty SizeProperty =
			DependencyProperty.Register("Size", typeof(double), typeof(Connector), new PropertyMetadata(15.0));

		public static readonly DependencyProperty FillProperty =
			DependencyProperty.Register("Fill", typeof(Brush), typeof(Connector), new PropertyMetadata(Brushes.Lavender));

		public static readonly DependencyProperty PositionProperty =
			DependencyProperty.Register("Position", typeof(Point), typeof(Connector), new PropertyMetadata(new Point()));

		#endregion

		public struct ConnectorInfo {
			public Connector Connector;
			public States Type;
			public Types State;
		}

		public enum Types {
			Numerical,
			Boolean,
			FloatingPoint,
			String,
			Object
		}

		public enum States {
			Input,
			Output,
			ZState
		}
	}
}
