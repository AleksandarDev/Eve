using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using Coding4Fun.Toolkit.Controls;
using EveWindowsPhone.Controls;
using EveWindowsPhone.RelayServiceReference;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace EveWindowsPhone.Pages.Modules.Ambiental {
	public partial class AmbientalView : PhoneApplicationPage {
		private const int CheckLightsTimerPeriod = 1000;
		private DispatcherTimer checkLightsTimer;


		public AmbientalView() {
			InitializeComponent();

			// Assign ViewModel
			if (this.ViewModel == null)
				if ((this.ViewModel = this.DataContext as AmbientalViewModel) == null)
					throw new NullReferenceException("Invalid ViewModel");
		}

		private void AmbientalViewOnLoaded(object sender, RoutedEventArgs e) {
			// This prevents exception when user navigates
			// off this page and page was loaded in same time
			// TODO Add this code to all pages
			if (this.ViewModel == null) return;

			// Create timer for checking lights
			this.checkLightsTimer = new DispatcherTimer() {
				Interval = TimeSpan.FromMilliseconds(CheckLightsTimerPeriod)
			};
			this.checkLightsTimer.Tick += InvokeRefreshLightsList;
			this.checkLightsTimer.Start();

			// Attach on list refresh event
			this.ViewModel.OnLightListRefreshed += s => this.CheckLights();
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e) {
			base.OnNavigatedFrom(e);

			if (this.checkLightsTimer != null) {
				this.checkLightsTimer.Tick -= InvokeRefreshLightsList;
				this.checkLightsTimer.Stop();
			}

			this.ViewModel = null;
		}

		private void CheckLights() {
			// Invoke this method with dispatcher if not already
			if (!this.Dispatcher.CheckAccess()) {
				this.Dispatcher.BeginInvoke(CheckLights);
				return;
			}

			// Check if any update is available
			if (this.ViewModel.Lights == null || this.ViewModel.Lights.Count == 0) {
				this.ContentPanel.Children.Clear();
				return;
			}

			// This prevents exception when user navigates 
			// off page and this method is being invoked
			// by dispatcher
			if (this.ViewModel == null)
				return;

			// Retrieve lights currently shown to user
			var children =
				this.ContentPanel.Children.OfType<AmbientalLightViewControl>();

			// List to save updated ids and those that need to be removed
			var updated = new List<int>();
			var toRemove = new List<int>();

			// Go through lights shown to user and update theirs state
			foreach (var child in children) {
				var light = child.Light;

				var update = this.ViewModel.Lights.First(l => l.ID == light.ID);
				if (update == null) {
					toRemove.Add(light.ID);
					continue;
				}

				updated.Add(light.ID);
				light.State = update.State;
				light.RValue = update.RValue;
				light.GValue = update.GValue;
				light.BValue = update.BValue; 
				light.AValue = update.AValue;

				child.UpdateColor();
			}

			// For all not updated, create new view
			foreach (var light in this.ViewModel.Lights) {
				if (!updated.Contains(light.ID)) {
					var control = new AmbientalLightViewControl() {
						Light = light
					};
					control.UpdateColor();
					control.OnSwitch +=
						amlvc => this.ViewModel.SwitchLight(amlvc.Light.ID, amlvc.Light.State);
					control.OnColorChanged +=
						alvc =>
						this.ViewModel.ChangeColor(alvc.Light.ID, alvc.Light.RValue,
												   alvc.Light.GValue, alvc.Light.BValue, alvc.Light.AValue);
					this.ContentPanel.Children.Add(control);
				}
			}

			// Remove lights that couldn't be updated
			// TODO Test this
			foreach (var id in toRemove) {
				this.ContentPanel.Children.Remove(
					this.ContentPanel.Children.OfType<AmbientalLightViewControl>()
						.First(alvc => alvc.Light.ID == id));
			}
		}

		private void InvokeRefreshLightsList(object sender, EventArgs args) {
			this.ViewModel.RefreshLightsListAsync();
		}

		#region Properties

		public AmbientalViewModel ViewModel { get; private set; }

		#endregion

		private void SettingsClick(object sender, EventArgs e) {
			this.ViewModel.AdvancedSettings();
		}

		private void HelpClick(object sender, EventArgs e) {
			(new WebBrowserTask() {
				Uri = new Uri("http://eve.toplek.net/help/module/ambiental/", UriKind.Absolute)
			}).Show();
		}
	}
}