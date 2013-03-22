using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using EveWindowsPhone.Adapters;
using EveWindowsPhone.Modules;
using EveWindowsPhone.ViewModels;

namespace EveWindowsPhone.Pages.Main {
	public class MainViewModel : NotificationObject {
		private INavigationServiceFacade navigationServiceFacade;


		public MainViewModel(INavigationServiceFacade navigationServiceFacade) {
			if (navigationServiceFacade == null) throw new ArgumentNullException("navigationServiceFacade");
			this.navigationServiceFacade = navigationServiceFacade;

			this.AvailableModules = new ObservableCollection<Module>();
		}


		#region Properties

		public ObservableCollection<Module> AvailableModules { get; private set; }

		#endregion

		public void NavigateTo(Module module) {
			// Navigate to module view
			if (!this.navigationServiceFacade.Navigate(new Uri(module.View, UriKind.Relative)))
				System.Diagnostics.Debug.WriteLine(String.Format("Couldn't navigate to \"{0}\" module's view", module.Name));
		}
	}
}
