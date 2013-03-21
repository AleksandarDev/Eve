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
	}
}
