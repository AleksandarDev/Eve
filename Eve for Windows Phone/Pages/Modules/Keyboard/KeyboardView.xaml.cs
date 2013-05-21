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

namespace EveWindowsPhone.Pages.Modules.Keyboard {
	public partial class KeyboardView : PhoneApplicationPage {
		public KeyboardView() {
			InitializeComponent();

			// Assign ViewModel
			if (this.ViewModel == null)
				if ((this.ViewModel = this.DataContext as KeyboardViewModel) == null)
					throw new NullReferenceException("Invalid ViewModel");
		}

		private void KeyboardViewOnLoaded(object sender, RoutedEventArgs e) {
			// Initial selected index call
			this.KeyboardGroupsSelectionChanged(this.GroupsPivot);
		}

		private void KeyboardGroupsSelectionChanged(object sender, SelectionChangedEventArgs e = null) {
			var pivot = sender as Pivot;
			if (pivot == null) return;
		
			if (pivot.SelectedIndex == 0) {
				// Apply tiles height
				TilesToSquares(this.GeneralContent.Children.OfType<Tile>());
			} else if (pivot.SelectedIndex == 1) {
				// Apply tiles height
				TilesToSquares(this.AdditionalContent.Children.OfType<Tile>());
				TilesToSquares(this.AdditionalFunctionContent.Children.OfType<Tile>());
			} else if (pivot.SelectedIndex == 2) {
				// Apply tiles height, but ignore multi row/column ones
				TilesToSquares(this.NumPadContent.Children
								   .OfType<Tile>()
								   .Where(t => Grid.GetColumnSpan(t) == 1 && Grid.GetRowSpan(t) == 1));
			} else if (pivot.SelectedIndex == 3) {
				// Apply tiles height
				TilesToSquares(this.ArrowsContent.Children.OfType<Tile>());
			}
		}

		private void ArrowTileClick(object sender, RoutedEventArgs e) {
			var position = GetTileGridPosition(sender as FrameworkElement);
		}

		private void NumPadTileClick(object sender, RoutedEventArgs e) {
			var position = GetTileGridPosition(sender as FrameworkElement);
		}

		private void AdditionalFunctionalTileClick(object sender, RoutedEventArgs e) {
			var position = GetTileGridPosition(sender as FrameworkElement);
		}

		private void AdditionalTileClick(object sender, RoutedEventArgs e) {
			var position = GetTileGridPosition(sender as FrameworkElement);
		}

		private void GeneralTileClick(object sender, RoutedEventArgs e) {
			var position = GetTileGridPosition(sender as FrameworkElement);
		}

		private static Point GetTileGridPosition(FrameworkElement element) {
			return new Point(Grid.GetColumn(element), Grid.GetRow(element));
		}

		private static void TilesToSquares(IEnumerable<Tile> tiles) {
			foreach (var tile in tiles)
				tile.Height = tile.ActualWidth;
		}

		#region Properties

		public KeyboardViewModel ViewModel { get; private set; }

		#endregion

		private void SettingsClick(object sender, EventArgs e) {
			this.ViewModel.AdvancedSettings();
		}

		private void HelpClick(object sender, EventArgs e) {
			(new WebBrowserTask() {
				Uri = new Uri("http://eve.toplek.net/help/module/keyboard/", UriKind.Absolute)
			}).Show();
		}
	}
}