using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using EveWindowsPhone.Adapters;
using EveWindowsPhone.Pages.Main;
using Funq;

namespace EveWindowsPhone.Services {
	public class ViewModelContainerService : IDisposable {
		private bool isDisposed;


		public ViewModelContainerService() {
			this.Container = new Container();
			this.InitializeContainer();
		}

		private void InitializeContainer() {
			this.Container.Register<INavigationServiceFacade>(
				c => new NavigationServiceFacade(((App) Application.Current).RootFrame));

			this.Container.Register(
				c => new MainViewModel(
					     c.Resolve<INavigationServiceFacade>()
						 )).ReusedWithin(ReuseScope.None);
		}

		#region Properties

		public Container Container { get; private set; }

		#endregion

		#region IDisposabble implementation

		public void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) {
			if (this.isDisposed) return;

			if (disposing) {
				this.Container.Dispose();
			}

			this.isDisposed = true;
		}

		#endregion
	}
}
