using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Coding4Fun.Toolkit.Controls;
using EveWindowsPhone.Controllers.ApplicationBarController;
using EveWindowsPhone.Modules;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace EveWindowsPhone.Pages.Main {
	public partial class MainView : PhoneApplicationPage {
		private const double TileMargins = 5;
		private const double TileImageMargins = 18;
		private const int TileRows = 2;

		private ApplicationBarController applicationBarController;

		private double tileHeight;
		private bool isEditingFavoriteModules;


		public MainView() {
			InitializeComponent();

			// Assign ViewModel
			if (this.ViewModel == null) 
				if ((this.ViewModel = this.DataContext as MainViewModel) == null) 
					throw new NullReferenceException("Invalid ViewModel");

			// All modules list 
			GestureService.GetGestureListener(ModulesList).Tap += ModulesListTap;
		}


		private void MainViewLoaded(object sender, RoutedEventArgs e) {
			this.LoadApplicationBar();
			this.LoadFavoriteModules();
		}

		private void PanoramaSelectionChanged(object sender, SelectionChangedEventArgs e) {
			this.UpdateApplicationBarPage();
		}

		#region ApplicationBar

		private void LoadApplicationBar() {
			// Application bar
			if (this.ApplicationBar == null)
				this.ApplicationBar = new ApplicationBar() { IsVisible = true, IsMenuEnabled = true };
			if (this.applicationBarController == null) {
				this.applicationBarController = new ApplicationBarController(this.ApplicationBar);
				this.PopulateApplicationBar();
			}
			this.UpdateApplicationBarPage();
		}

		private void PopulateApplicationBar() {
			// All Modules page
			var allModuesPage = new ApplicationBarPage(ApplicationBarMode.Default);
			allModuesPage.AddIconButton("edit favorites", new Uri("/Resources/Images/Heart.png", UriKind.Relative),
										() => { this.SetEditFavoriteModules(!this.isEditingFavoriteModules); });
			allModuesPage.AddIconButton("get more", new Uri("/Resources/Images/Shopping Bag.png", UriKind.Relative), () => { });
			this.applicationBarController.AddPage("all modules", allModuesPage);
		}

		private void UpdateApplicationBarPage() {
			var selectedItem = this.LayoutRoot.SelectedItem as PanoramaItem;
			if (selectedItem == null) return;

			this.applicationBarController.SetPageBar(selectedItem.Header.ToString());
		}

		#endregion

		#region Favorite modules

		private void LoadFavoriteModules() {
			// Initial variable values
			this.tileHeight = this.FavoriteModulesGrid.ActualHeight/TileRows - 2*TileMargins;

			// Attach on favorite modules collection changed
			this.ViewModel.FavoriteModules.CollectionChanged += (sender, args) => { this.UpdateFavoriteModulesLayout(); };

			// Initial layout for favorite modules
			this.UpdateFavoriteModulesLayout();
		}

		private void UpdateFavoriteModulesLayout() {
			// Clear all previous data
			this.FavoriteModulesGrid.Children.Clear();
			this.FavoriteModulesGrid.RowDefinitions.Clear();
			this.FavoriteModulesGrid.ColumnDefinitions.Clear();

			// Build favorite modules grid (horizontal)
			int numColumns = (int)Math.Ceiling((double)this.ViewModel.FavoriteModules.Count / TileRows);

			// Generate row and column definitions
			for (int index = 0; index < TileRows; index++) {
				this.FavoriteModulesGrid.RowDefinitions.Add(
					new RowDefinition() { Height = new GridLength(tileHeight + 2 * TileMargins) });
			}
			for (int index = 0; index < numColumns; index++) {
				this.FavoriteModulesGrid.ColumnDefinitions.Add(
					new ColumnDefinition() { Width = new GridLength(tileHeight + 2 * TileMargins) });
			}

			// Generate tiles
			for (int index = 0; index < this.ViewModel.FavoriteModules.Count; index++) {
				var module = this.ViewModel.FavoriteModules.ElementAt(index);
				var tile = MainView.CreateTile(
					module.Module.Name, module.Module.Image,
					tileHeight, tileHeight, TileMargins);
				Grid.SetRow(tile, index % TileRows); //http://www.youtube.com/watch?v=TiWFR1qxatc
				Grid.SetColumn(tile, index / TileRows);
				this.FavoriteModulesGrid.Children.Add(tile);
			}
		}

		private static Tile CreateTile(string text, string image, double height, double width, double margins) {
			return new Tile() {
				Label = text,
				Content = new Image() {
					Source = new BitmapImage(new Uri(image, UriKind.RelativeOrAbsolute)),
					Margin = new Thickness(TileImageMargins)
				},
				Width = width,
				Height = height,
				Margin = new Thickness(margins)
			};
		}

		public void SetEditFavoriteModules(bool mode) {
			this.isEditingFavoriteModules = mode;
			this.ViewModel.SetModulesIsEditing(mode);
			if (!mode) this.ViewModel.SaveFavorites();
		}

		#endregion

		#region All modules

		private void ModulesListTap(object sender, Microsoft.Phone.Controls.GestureEventArgs e) {
			if (sender is ListBox) {
				var module = (sender as ListBox).SelectedItem as ModuleModel;
				if (module != null)
					this.ViewModel.NavigateTo(module.Module);
			}
		}

		#endregion




		#region Properties

		public MainViewModel ViewModel { get; private set; }

		#endregion
	}
}