using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace EveWindowsPhone.Pages.Modules.Play {
	public partial class PlayView : PhoneApplicationPage {
		public PlayView() {
			InitializeComponent();

			// Assign ViewModel
			if (this.ViewModel == null)
				if ((this.ViewModel = this.DataContext as PlayViewModel) == null)
					throw new NullReferenceException("Invalid ViewModel");
		}


		#region Properties

		public PlayViewModel ViewModel { get; private set; }

		#endregion
	}
}