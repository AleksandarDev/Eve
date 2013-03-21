using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveWindowsPhone.Adapters {
	public interface INavigationServiceFacade {
		bool CanGoBack { get; }
		bool CanGoForward { get; }
		Uri Current { get; }

		bool Navigate(Uri destination);
		void GoBack();
		void GoForward();
	}
}
