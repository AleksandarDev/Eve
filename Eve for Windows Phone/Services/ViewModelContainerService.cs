using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using EveWindowsPhone.Adapters;
using EveWindowsPhone.Pages.AdvancedSettings;
using EveWindowsPhone.Pages.ChangeClient;
using EveWindowsPhone.Pages.Main;
using EveWindowsPhone.Pages.Modules.Lights;
using EveWindowsPhone.Pages.Modules.Play;
using EveWindowsPhone.Pages.Modules.Touch;
using Funq;

namespace EveWindowsPhone.Services {
	public class ViewModelContainerService : IDisposable {
		private bool isDisposed;


		public ViewModelContainerService() {
			this.Container = new Container();
			this.InitializeContainer();
		}

		private void InitializeContainer() {
			// Service facades
			this.Container.Register<INavigationServiceFacade>(
				c => new NavigationServiceFacade(((App) Application.Current).RootFrame));
			this.Container.Register<IIsolatedStorageServiceFacade>(
				c => new IsolatedStorageServiceFacade());
			this.Container.Register<IRelayServiceFacade>(
				c => new RelayServiceFacade());

			// Application Pages
			this.Container.Register(
				c => new MainViewModel(
					     c.Resolve<INavigationServiceFacade>(),
						 c.Resolve<IIsolatedStorageServiceFacade>()
						 )).ReusedWithin(ReuseScope.None);
			this.Container.Register(
				c => new CreateClientViewModel(
					     c.Resolve<INavigationServiceFacade>(),
					     c.Resolve<IIsolatedStorageServiceFacade>()
					     )).ReusedWithin(ReuseScope.None);
			this.Container.Register(
				c => new AdvancedSettingsViewModel(
					     c.Resolve<INavigationServiceFacade>(),
					     c.Resolve<IIsolatedStorageServiceFacade>()
					     )).ReusedWithin(ReuseScope.None);

			// Modules
			this.Container.Register(
				c => new TouchViewModel(
					     c.Resolve<INavigationServiceFacade>(),
						 c.Resolve<IIsolatedStorageServiceFacade>(),
						 c.Resolve<IRelayServiceFacade>()
					     )).ReusedWithin(ReuseScope.None);
			this.Container.Register(
				c => new LightsViewModel(
					     c.Resolve<INavigationServiceFacade>(),
					     c.Resolve<IIsolatedStorageServiceFacade>()
					     )).ReusedWithin(ReuseScope.None);
			this.Container.Register(
				c => new PlayViewModel(
					     c.Resolve<INavigationServiceFacade>(),
					     c.Resolve<IIsolatedStorageServiceFacade>()
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
