using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Coding4Fun.Toolkit.Controls;
using EveWindowsPhone.RelayServiceReference;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace EveWindowsPhone.Pages.Modules.Ambiental {
	public partial class AmbientalView : PhoneApplicationPage {
		private const int CheckLightsTimerPeriod = 1000;
		private Timer checkLightsTimer;


		public AmbientalView() {
			InitializeComponent();

			// Assign ViewModel
			if (this.ViewModel == null)
				if ((this.ViewModel = this.DataContext as AmbientalViewModel) == null)
					throw new NullReferenceException("Invalid ViewModel");
		}

		private void AmbientalViewOnLoaded(object sender, RoutedEventArgs e) {
			// Create timer for checking lights
			this.checkLightsTimer = new Timer(state => this.CheckLights(),
											  null, 0, CheckLightsTimerPeriod);
		}

		private void CheckLights() {
			// Invoke this method with dispatcher if not already
			if (!this.Dispatcher.CheckAccess()) {
				this.Dispatcher.BeginInvoke(CheckLights);
				return;
			}

			// TODO Improve this, reuse objects
			this.ContentPanel.Children.Clear();

			foreach (var light in this.ViewModel.GetLights()) {
				this.ContentPanel.Children.Add(
					this.CreateAmbientalLightControl(light));
			}
		}

		private UIElement CreateAmbientalLightControl(AmbientalLight light) {
			// Create switch button and asign state
			var lightSwitch = new ToggleSwitch() {
				Header = light.Alias,
				IsChecked = light.State
			};

			// Create color picker
			var colorPicker = new ColorSlider() {
				Orientation = System.Windows.Controls.Orientation.Horizontal,
				Height = 35.0,
				Color = new Color() {
					R = light.RValue,
					G = light.GValue,
					B = light.BValue,
					A = light.AValue
				}
			};

			// Change switch color on color change
			colorPicker.ColorChanged += (s, color) => lightSwitch.SwitchForeground = new SolidColorBrush(color);

			// Set initial color
			lightSwitch.SwitchForeground = new SolidColorBrush(colorPicker.Color);

			// Create container
			var container = new StackPanel() {
				Orientation = System.Windows.Controls.Orientation.Vertical
			};
			container.Children.Add(lightSwitch);
			container.Children.Add(colorPicker);

			return container;
		}

		#region Properties

		public AmbientalViewModel ViewModel { get; private set; }

		#endregion
	}
}