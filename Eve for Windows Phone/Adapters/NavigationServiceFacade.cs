using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Phone.Controls;

namespace EveWindowsPhone.Adapters {
	public class NavigationServiceFacade : INavigationServiceFacade {
		private readonly PhoneApplicationFrame frame;


		public NavigationServiceFacade(PhoneApplicationFrame frame) {
			if (frame == null)
				throw new ArgumentNullException("frame");

			this.frame = frame;
		}

		public bool Navigate(Uri destination) {
			return this.frame.Navigate(destination);
		}

		public void GoBack() {
			this.frame.GoBack();
		}

		public void GoForward() {
			this.frame.GoForward();
		}

		#region Properties

		public bool CanGoBack {
			get { return this.frame.CanGoBack; }
		}

		public bool CanGoForward {
			get { return this.frame.CanGoForward; }
		}

		public Uri Current {
			get { return this.frame.CurrentSource; }
		}

		#endregion
	}
}
