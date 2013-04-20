using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace EveWindowsPhone.Pages.ChangeClient {
	public partial class CreateClientView : PhoneApplicationPage {
		public CreateClientView() {
			InitializeComponent();

			// Assign ViewModel
			if (this.ViewModel == null)
				if ((this.ViewModel = this.DataContext as CreateClientViewModel) == null)
					throw new NullReferenceException("Invalid ViewModel");
		}


		#region Properties

		public CreateClientViewModel ViewModel { get; private set; }

		#endregion
	}
}