using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Coding4Fun.Toolkit.Controls;
using Eve.Diagnostics.Logging;
using EveWindowsPhone.Controllers.ApplicationBarController;
using EveWindowsPhone.Modules;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using EveWindowsPhone.RelayServiceReference;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.UserData;

namespace EveWindowsPhone.Pages.Main {
	public partial class MainView : PhoneApplicationPage {
		private readonly Log.LogInstance log =
			new Log.LogInstance(typeof(MainView));

		private const double TileMargins = 5;
		private const double TileImageMargins = 18;

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
			GestureService.GetGestureListener(OwnedModulesList).Tap += ModulesListTap;
		}


		private void MainViewLoaded(object sender, RoutedEventArgs e) {
			this.LoadApplicationBar();
			this.LoadFavoriteModules();
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e) {
			base.OnNavigatedTo(e);

			// Update settings on reload
			this.ViewModel.LoadSettings();
			this.ViewModel.LoadModules();
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
			// Favorite modules page
			var favoriteModulesPage = new ApplicationBarPage(ApplicationBarMode.Default);
			favoriteModulesPage.AddIconButton("settings...",
											  new Uri("/Resources/Images/Settings-02.png", UriKind.Relative), this.ViewModel.AdvancedSettingsFavorites);
			this.applicationBarController.AddPage("favorite", favoriteModulesPage);

			// All Modules page
			var allModuesPage = new ApplicationBarPage(ApplicationBarMode.Default);
			allModuesPage.AddIconButton("edit favorites", new Uri("/Resources/Images/Heart.png", UriKind.Relative),
										() => this.SetEditFavoriteModules(!this.isEditingFavoriteModules));
			allModuesPage.AddIconButton("get more", new Uri("/Resources/Images/Shopping Bag.png", UriKind.Relative), () => { });
			this.applicationBarController.AddPage("all modules", allModuesPage);

			// Options page
			var optionsPage = new ApplicationBarPage(ApplicationBarMode.Default);
			optionsPage.AddIconButton("about", new Uri("/Resources/Images/About.png", UriKind.Relative),
									  this.ShowAboutPrompt);
			this.applicationBarController.AddPage("options", optionsPage);
		}

		private void UpdateApplicationBarPage() {
			var selectedItem = this.LayoutRoot.SelectedItem as PanoramaItem;
			if (selectedItem == null) return;

			// Set current page as page header
			this.applicationBarController.SetPageBar(selectedItem.Header.ToString());
		}

		#endregion

		#region Favorite modules

		private void LoadFavoriteModules() {
			if (Double.IsNaN(this.FavoriteModulesGrid.ActualHeight) ||
				(int)this.FavoriteModulesGrid.ActualHeight == 0) return;

			// Initial variable values
			this.tileHeight =
				(this.FavoriteModulesGrid.ActualHeight -
				 this.FavoriteModulesGrid.Margin.Bottom -
				 this.FavoriteModulesGrid.Margin.Top) / this.ViewModel.TileRows -
				2 * TileMargins;

			// Attach on favorite modules collection changed
			this.ViewModel.FavoriteModules.CollectionChanged += (sender, args) => this.UpdateFavoriteModulesLayout();

			// Initial layout for favorite modules
			this.UpdateFavoriteModulesLayout();
		}

		private void UpdateFavoriteModulesLayout() {
			if (this.ViewModel.FavoriteModules.Count == 0) return;

			// Clear all previous data
			this.FavoriteModulesGrid.Children.Clear();
			this.FavoriteModulesGrid.RowDefinitions.Clear();
			this.FavoriteModulesGrid.ColumnDefinitions.Clear();

			// Build favorite modules grid (horizontal)
			int numColumns = (int)Math.Ceiling((double)this.ViewModel.FavoriteModules.Count / this.ViewModel.TileRows);

			// Generate row and column definitions
			for (int index = 0; index < this.ViewModel.TileRows; index++) {
				this.FavoriteModulesGrid.RowDefinitions.Add(
					new RowDefinition() { Height = new GridLength(this.tileHeight + 2 * TileMargins) });
			}
			for (int index = 0; index < numColumns; index++) {
				this.FavoriteModulesGrid.ColumnDefinitions.Add(
					new ColumnDefinition() { Width = new GridLength(this.tileHeight + 2 * TileMargins) });
			}

			// Generate tiles
			for (int index = 0; index < this.ViewModel.FavoriteModules.Count; index++) {
				var module = this.ViewModel.FavoriteModules.ElementAt(index);
				var tile = this.CreateTile(module, tileHeight, tileHeight, TileMargins);
				Grid.SetRow(tile, index % this.ViewModel.TileRows);
				Grid.SetColumn(tile, index / this.ViewModel.TileRows);
				this.FavoriteModulesGrid.Children.Add(tile);
			}
		}

		private Tile CreateTile(ModuleModel module, double height, double width, double margins) {
			var tile = new Tile() {
				Label = module.ModuleAttribute.Name,
				Content = new Image() {
					Source =
						new BitmapImage(new Uri(module.ModuleAttribute.Image,
												UriKind.RelativeOrAbsolute)),
					Margin = new Thickness(TileImageMargins)
				},
				Width = width,
				Height = height,
				Margin = new Thickness(margins),
				Foreground = new SolidColorBrush(Colors.White)
			};
			tile.Tap += (s, e) => this.ViewModel.NavigateTo(module.ModuleAttribute);
			return tile;
		}

		public void SetEditFavoriteModules(bool mode) {
			this.isEditingFavoriteModules = mode;
			this.ViewModel.SetModulesEditMode(mode);
			if (!mode) this.ViewModel.SaveFavorites();
		}

		#endregion

		#region All modules

		private void ModulesListTap(object sender, Microsoft.Phone.Controls.GestureEventArgs e) {
			if (sender is ListBox) {
				var module = (sender as ListBox).SelectedItem as ModuleModel;
				if (module != null)
					this.ViewModel.NavigateTo(module.ModuleAttribute);
			}
		}

		#endregion

		#region Options page

		private void ClientDevicesListPickerOnSelectionChanged(object sender, SelectionChangedEventArgs e) {
			//throw new NotImplementedException();
		}

		private void ShowAboutPrompt() {
			this.LayoutRoot.SetValue(Panorama.SelectedIndexProperty, 0);
			this.LayoutRoot.Measure(new Size());

			var aboutPrompt = new AboutPrompt() {
				Title = "SyncUp for Eve",
				VersionNumber = "Version 0.1 Alpha",
				Footer = "Visit us at http://eve.toplek.net"
			};
			aboutPrompt.Show();
		}

		private void AdvancedSettingOnClick(object sender, RoutedEventArgs e) {
			this.ViewModel.AdvancedSettings();
		}

		#endregion

		#region Properties

		public MainViewModel ViewModel { get; private set; }

		#endregion
	}
}