using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Eve.Diagnostics.Logging;
using EveWindowsPhone.Adapters;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace EveWindowsPhone.Pages.AdvancedSettings {
	public partial class AdvancedSettingsView : PhoneApplicationPage {
		private readonly Log.LogInstance log =
			new Log.LogInstance(typeof(AdvancedSettingsView));


		public AdvancedSettingsView() {
			InitializeComponent();

			// Assign ViewModel
			if (this.ViewModel == null)
				if ((this.ViewModel = this.DataContext as AdvancedSettingsViewModel) == null)
					throw new NullReferenceException("Invalid ViewModel");
		}

		private void AdvancedSettingsViewOnLoaded(object sender, RoutedEventArgs e) {
			// Check if request for certain page is sent
			if (NavigationContext.QueryString.ContainsKey("Index")) {
				int index = 0;
				Int32.TryParse(NavigationContext.QueryString["Index"], out index);
				this.LayoutRoot.SelectedIndex = index;

				this.log.Info("Changing view to page {0}", index);
			}
		}

		private void ClearFavoriteModules(object sender, RoutedEventArgs e) {
			this.ViewModel.ClearFavoriteModules();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e) {
			base.OnNavigatedTo(e);

			// Update settings on reload
			this.ViewModel.LoadSettings();
		}

		public static void NavigateWithIndex(INavigationServiceFacade navigationServiceFacede, int landOnPageIndex = 0) {
			navigationServiceFacede.Navigate(
				new Uri(
					"/Pages/AdvancedSettings/AdvancedSettingsView.xaml?Index=" +
					landOnPageIndex, UriKind.Relative));
		}

		#region Properties

		public AdvancedSettingsViewModel ViewModel { get; private set; }

		#endregion
	}
}