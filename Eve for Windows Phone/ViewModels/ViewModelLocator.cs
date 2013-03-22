using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveWindowsPhone.Pages.Main;
using EveWindowsPhone.Pages.Modules.Touch;
using EveWindowsPhone.Services;

namespace EveWindowsPhone.ViewModels {
	public class ViewModelLocator : IDisposable {
		private readonly ViewModelContainerService viewModelContainerService;


		public ViewModelLocator() {
			this.viewModelContainerService = new ViewModelContainerService();
		}


		#region Properties

		public MainViewModel MainViewModel {
			get { return this.viewModelContainerService.Container.Resolve<MainViewModel>(); }
		}

		public TouchViewModel TouchViewModel {
			get { return this.viewModelContainerService.Container.Resolve<TouchViewModel>(); }
		}

		#endregion

		#region IDisposable implementation

		private bool isDisposed;

		public void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) {
			if (this.isDisposed) return;

			if (disposing) {
				this.viewModelContainerService.Dispose();
			}

			this.isDisposed = true;
		}

		#endregion
	}
}
