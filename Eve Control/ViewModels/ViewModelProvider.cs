using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EveControl.Windows.Main;
using Microsoft.Practices.Unity;
using EveControl.Services;

namespace EveControl.ViewModels {
	public class ViewModelProvider : IDisposable {
		private bool isDisposed;
		private readonly ViewModelContainerService containerService;


		public ViewModelProvider() {
			this.containerService = new ViewModelContainerService();
		}


		#region Properties

		public MainViewModel MainViewModel {
			get { return this.containerService.Container.Resolve<MainViewModel>(); }
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
				this.containerService.Dispose();
			}

			this.isDisposed = true;
		}

		#endregion
	}
}
