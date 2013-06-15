using System;
using EveControl.Adapters;
using Microsoft.Practices.Unity;

namespace EveControl.Services {
	public class ServiceFacadeContainerService : IDisposable {
		private readonly UnityContainer container;

		
		public ServiceFacadeContainerService() {
			this.container = new UnityContainer();
			this.InitializeContainer();
		}

		private void InitializeContainer() {
			// Register service facades
			this.container.RegisterType<
				IServerCallbackServiceFacade,
				ServerCallbackServiceFacade>();
			this.container.RegisterType<
				IServerServiceFacade,
				InternetServerServiceFacade>();
		}

		#region Properties

		public UnityContainer Container {
			get { return this.container; }
		}

		#endregion

		#region IDisposable implementation

		private bool isDisposed;

		public void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected async virtual void Dispose(bool disposing) {
			if (this.isDisposed) return;

			if (disposing) {
				this.container.Dispose();
			}

			this.isDisposed = true;
		}

		#endregion
	}
}