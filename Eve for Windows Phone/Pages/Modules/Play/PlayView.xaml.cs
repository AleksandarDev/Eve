using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace EveWindowsPhone.Pages.Modules.Play {
	public partial class PlayView : PhoneApplicationPage {
		public PlayView() {
			InitializeComponent();

			// Assign ViewModel
			if (this.ViewModel == null)
				if ((this.ViewModel = this.DataContext as PlayViewModel) == null)
					throw new NullReferenceException("Invalid ViewModel");
		}

		private void PlayViewOnLoaded(object sender, RoutedEventArgs e) {
			// Apply tile height
			TilesToSquares(this.PlayNavigationContent.Children
				.OfType<Tile>()
				.Where(t => Grid.GetColumnSpan(t) == 1 && Grid.GetRowSpan(t) == 1));
		}

		private void NavigationTileClick(object sender, RoutedEventArgs e) {
			throw new NotImplementedException();
		}

		private static void TilesToSquares(IEnumerable<Tile> tiles) {
			foreach (var tile in tiles)
				tile.Height = tile.ActualWidth;
		}

		#region Properties

		public PlayViewModel ViewModel { get; private set; }

		#endregion

		private void SettingsClick(object sender, EventArgs e) {
			this.ViewModel.AdvancedSettings();
		}

		private void HelpClick(object sender, EventArgs e) {
			(new WebBrowserTask() {
				Uri = new Uri("http://eve.toplek.net/help/module/lights/", UriKind.Absolute)
			}).Show();
		}
	}
}