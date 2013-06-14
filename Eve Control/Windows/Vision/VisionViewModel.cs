using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eve.Core.ViewModels;

namespace EveControl.Windows.Vision {
	public class VisionViewModel : NotificationObject, IDisposable {
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
