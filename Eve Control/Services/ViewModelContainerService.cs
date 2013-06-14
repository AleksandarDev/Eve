using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EveControl.Adapters;
using EveControl.Windows.Main;
using Microsoft.Practices.Unity;

namespace EveControl.Services {
	public class ViewModelContainerService : IDisposable {
		private bool isDisposed;
		private readonly UnityContainer container;


		public ViewModelContainerService() {
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

			// Register ViewModels
			this.container.RegisterInstance(
				new MainViewModel(
					this.container.Resolve<IServerServiceFacade>()));
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
				this.container.Dispose();
			}

			this.isDisposed = true;
		}

		#endregion
	}
}
