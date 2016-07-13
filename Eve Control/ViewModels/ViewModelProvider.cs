using System;
using EveControl.Windows.Main;
using EveControl.Windows.Chrome;
using EveControl.Windows.FaceController;
using EveControl.Windows.FridgeManager;
using EveControl.Windows.Log;
using EveControl.Windows.Vision;
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

		public FaceControllerViewModel FaceControllerViewModel {
			get { return this.containerService.Container.Resolve<FaceControllerViewModel>(); }
		}

		public FridgeManagerViewModel FridgeManagerViewModel {
			get { return this.containerService.Container.Resolve<FridgeManagerViewModel>(); }
		}

		public LogViewModel LogViewModel {
			get { return this.containerService.Container.Resolve<LogViewModel>(); }
		}

		public VisionViewModel VisionViewModel {
			get { return this.containerService.Container.Resolve<VisionViewModel>(); }
		}

		public ChromeViewModel ChromeViewModel {
			get { return this.containerService.Container.Resolve<ChromeViewModel>(); }
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
