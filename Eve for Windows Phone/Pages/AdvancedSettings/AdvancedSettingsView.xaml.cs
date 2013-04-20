using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace EveWindowsPhone.Pages.AdvancedSettings {
	public partial class AdvancedSettingsView : PhoneApplicationPage {
		public AdvancedSettingsView() {
			InitializeComponent();

			// Assign ViewModel
			if (this.ViewModel == null)
				if ((this.ViewModel = this.DataContext as AdvancedSettingsViewModel) == null)
					throw new NullReferenceException("Invalid ViewModel");
		}


		private void AdvancedSettingsViewOnLoaded(object sender, RoutedEventArgs e) {
			this.FavoriteRowsListPicker.Items.Add(2);
			this.FavoriteRowsListPicker.Items.Add(3);
			this.FavoriteRowsListPicker.Items.Add(4);
		}

		#region Properties

		public AdvancedSettingsViewModel ViewModel { get; private set; }

		#endregion
	}
}