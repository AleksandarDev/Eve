using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Eve.UI.Controls.DiagramDesigner {
	public class DiagramItem : ContentControl, ISelectable {
		private DiagramCanvas parentDiagram;
		 

		public DiagramItem() : base() {
			this.LayoutUpdated += DiagramItemLayoutUpdated;
		}

		void DiagramItemLayoutUpdated(object sender, EventArgs e) {
			this.parentDiagram = VisualTreeHelper.GetParent(this) as DiagramCanvas;
			if (this.parentDiagram == null)
				throw new NullReferenceException("Can't find parent DiagramCanvas");
		}

		protected override void OnPreviewMouseDown(System.Windows.Input.MouseButtonEventArgs e) {
			base.OnPreviewMouseDown(e);

			if (this.ParentDiagram != null) {
				if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None) {
					this.IsSelected = !this.IsSelected;
					this.ParentDiagram.SetAsSelected(this, this.IsSelected);
				}
				else {
					if (!this.IsSelected) {
						// Deselect all DiagramItems on diagram and set this as selected
						this.ParentDiagram.DeselectAll();
						this.IsSelected = true;

						// Set this element on top
						Canvas.SetZIndex(this, DiagramCanvas.DiagramItemZIndexSelected);
					}
				}

				this.Focus();
			}

			e.Handled = false;
		}

		private static void IsSelectedChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
			var control = sender as DiagramItem;
			if (control == null) return;

			var diagram = VisualTreeHelper.GetParent(control) as DiagramCanvas;
			if (diagram == null) return;

			diagram.SetAsSelected(control, control.IsSelected);
		}

		#region Properties

		public bool IsSelected {
			get { return (bool)GetValue(IsSelectedProperty); }
			set { SetValue(IsSelectedProperty, value); }
		}

		public string Header {
			get { return (string)GetValue(HeaderProperty); }
			set { SetValue(HeaderProperty, value); }
		}

		public DiagramCanvas ParentDiagram {
			get { return this.parentDiagram; }
		}

		public static readonly DependencyProperty HeaderProperty =
			DependencyProperty.Register("Header", typeof(string), typeof(DiagramItem), new PropertyMetadata("UnnamedDiagramItem"));

		public static readonly DependencyProperty IsSelectedProperty =
			DependencyProperty.Register("IsSelected", typeof(bool), typeof(DiagramItem), new PropertyMetadata(false,IsSelectedChangedCallback));

		#endregion

		internal void EnableInputs(Connector source) {
			
		}

		internal void DisableInputs() {
			throw new NotImplementedException();
		}
	}
}
