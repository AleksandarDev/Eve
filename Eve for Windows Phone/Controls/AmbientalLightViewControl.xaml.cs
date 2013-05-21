using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Coding4Fun.Toolkit.Controls;
using EveWindowsPhone.RelayServiceReference;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace EveWindowsPhone.Controls {
	public partial class AmbientalLightViewControl : UserControl {
		public event AmbientalLightViewControlEventHandler OnSwitch;
		public event AmbientalLightViewControlEventHandler OnColorChanged;


		public AmbientalLightViewControl() {
			InitializeComponent();
		}


		public void OnColorSliderChangeColor(object sender, Color color) {
			// Check ig method needs to be called from main thread
			if (!this.Dispatcher.CheckAccess()) {
				this.OnColorSliderChangeColor(sender, color);
				return;
			}

			this.Light.RValue = color.R;
			this.Light.GValue = color.G;
			this.Light.BValue = color.B;
			this.Light.AValue = color.A;

			this.ToggleSwitch.SwitchForeground = new SolidColorBrush(color);

			if (this.OnColorChanged != null)
				this.OnColorChanged(this);
		}

		public void UpdateColor() {
			var color = new Color() {
				R = this.Light.RValue,
				G = this.Light.GValue,
				B = this.Light.BValue,
				A = this.Light.AValue
			};
			this.ToggleSwitch.SwitchForeground = new SolidColorBrush(color);
			this.ColorSlider.Color = color;
		}

		private void ToggleSwitchOnChecked(object sender, RoutedEventArgs e) {
			if (this.OnSwitch != null)
				this.OnSwitch(this);
		}

		#region Properties

		public AmbientalLight Light {
			get { return (AmbientalLight)GetValue(LightProperty); }
			set { SetValue(LightProperty, value); }
		}

		public static readonly DependencyProperty LightProperty =
			DependencyProperty.Register("Light", typeof(AmbientalLight),
										typeof(AmbientalLightViewControl),
										new PropertyMetadata(default(AmbientalLight)));

		#endregion
	}
}
