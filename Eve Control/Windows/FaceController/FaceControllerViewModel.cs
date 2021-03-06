﻿using System;
using EveControl.Adapters;
using Microsoft.Practices.Prism.ViewModel;

namespace EveControl.Windows.FaceController {
	public class FaceControllerViewModel : NotificationObject, IDisposable {
		private readonly IServerServiceFacade serverServiceFacade;

		public FaceControllerViewModel(IServerServiceFacade serverServiceFacade) {
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
