using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveWindowsPhone.Pages.AdvancedSettings;
using EveWindowsPhone.Pages.ChangeClient;
using EveWindowsPhone.Pages.Main;
using EveWindowsPhone.Pages.Modules.Lights;
using EveWindowsPhone.Pages.Modules.Play;
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

		public CreateClientViewModel CreateClientViewModel {
			get { return this.viewModelContainerService.Container.Resolve<CreateClientViewModel>(); }
		}

		public AdvancedSettingsViewModel AdvancedSettingsViewModel {
			get { return this.viewModelContainerService.Container.Resolve<AdvancedSettingsViewModel>(); }
		}

		public TouchViewModel TouchViewModel {
			get { return this.viewModelContainerService.Container.Resolve<TouchViewModel>(); }
		}

		public LightsViewModel LightsViewModel {
			get { return this.viewModelContainerService.Container.Resolve<LightsViewModel>(); }
		}

		public PlayViewModel PlayViewModel {
			get { return this.viewModelContainerService.Container.Resolve<PlayViewModel>(); }
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
