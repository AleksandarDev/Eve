using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace EveWindowsPhone.Pages.Modules.Lights {
	public partial class LightsView : PhoneApplicationPage {
		public LightsView() {
			InitializeComponent();

			// Assign ViewModel
			if (this.ViewModel == null)
				if ((this.ViewModel = this.DataContext as LightsViewModel) == null)
					throw new NullReferenceException("Invalid ViewModel");
		}


		#region Properties

		public LightsViewModel ViewModel { get; private set; }

		#endregion
	}
}