using System;
using Eve.Core.ViewModels;
using EveControl.Adapters;

namespace EveControl.Windows.Vision {
	public class VisionViewModel : NotificationObject, IDisposable {
		private readonly IServerServiceFacade serverServiceFacade;
		
		public VisionViewModel(IServerServiceFacade serverServiceFacade) {
			if (serverServiceFacade == null)
				throw new ArgumentNullException("serverServiceFacade");

			this.serverServiceFacade = serverServiceFacade;
		}

		#region IDisposable implementation

		private bool isDisposed;

		public void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected async virtual void Dispose(bool disposing) {
			if (this.isDisposed) return;

			if (disposing) {
				// Dispose any disposable objects HERE
			}

			this.isDisposed = true;
		}

		#endregion
	}
}
