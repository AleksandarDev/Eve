using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EveControl.Adapters;
using Microsoft.Practices.Prism.ViewModel;

namespace EveControl.Windows.FridgeManager {
	public class FridgeManagerViewModel : NotificationObject, IDisposable {
		private readonly IServerServiceFacade serverServiceFacade;

		public FridgeManagerViewModel(IServerServiceFacade serverServiceFacade) {
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
