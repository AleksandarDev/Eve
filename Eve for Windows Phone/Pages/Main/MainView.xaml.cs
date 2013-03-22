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
using System.Windows.Shapes;
using EveWindowsPhone.Modules;
using Microsoft.Phone.Controls;

namespace EveWindowsPhone.Pages.Main {
	public partial class MainView : PhoneApplicationPage {
		public MainView() {
			InitializeComponent();

			// Assign ViewModel
			if (this.ViewModel == null) 
				if ((this.ViewModel = this.DataContext as MainViewModel) == null) 
					throw new NullReferenceException("Invalid ViewModel");
		}


		private void MainViewLoaded(object sender, RoutedEventArgs e) {
			GestureService.GetGestureListener(ModulesList).Tap += ModulesListTap;
		}

		private void ModulesListTap(object sender, Microsoft.Phone.Controls.GestureEventArgs e) {
			if (sender is ListBox) {
				var module = (sender as ListBox).SelectedItem as Module;
				if (module != null)
					this.ViewModel.NavigateTo(module);
			}
		}


		#region Properties

		public MainViewModel ViewModel { get; private set; }

		#endregion
	}
}