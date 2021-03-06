﻿using System;
using EveControl.Adapters;
using EveControl.Windows.Main;
using EveControl.Windows.Chrome;
using EveControl.Windows.FaceController;
using EveControl.Windows.FridgeManager;
using EveControl.Windows.Log;
using EveControl.Windows.Vision;
using Microsoft.Practices.Unity;

namespace EveControl.Services {
	public class ViewModelContainerService : IDisposable {
		private bool isDisposed;
		private readonly ServiceFacadeContainerService servicesContainer;
		private readonly UnityContainer container;


		public ViewModelContainerService() {
			this.container = new UnityContainer();
			this.servicesContainer = new ServiceFacadeContainerService();
			this.InitializeContainer();
		}

		private void InitializeContainer() {
			// Register ViewModels
			this.container.RegisterInstance(
				new MainViewModel(
					this.servicesContainer.Container.Resolve<IServerServiceFacade>()));
			this.container.RegisterInstance(
				new ChromeViewModel(
					this.servicesContainer.Container.Resolve<IServerServiceFacade>()));
			this.container.RegisterInstance(
				new FaceControllerViewModel(
					this.servicesContainer.Container.Resolve<IServerServiceFacade>()));
			this.container.RegisterInstance(
				new FridgeManagerViewModel(
					this.servicesContainer.Container.Resolve<IServerServiceFacade>()));
			this.container.RegisterInstance(
				new LogViewModel(
					this.servicesContainer.Container.Resolve<IServerServiceFacade>()));
			this.container.RegisterInstance(
				new VisionViewModel(
					this.servicesContainer.Container.Resolve<IServerServiceFacade>()));
		}


		#region Properties

		public UnityContainer Container {
			get { return this.container; }
		}

		#endregion

		#region IDisposabble implementation

		public void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) {
			if (this.isDisposed) return;

			if (disposing) {
				this.servicesContainer.Dispose();
				this.container.Dispose();
			}

			this.isDisposed = true;
		}

		#endregion
	}
}
