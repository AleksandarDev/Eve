using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eve.API;
using EveControl.Adapters;
using Microsoft.Practices.Prism.ViewModel;

namespace EveControl.Windows.MainWindow {
	public class MainViewModel : NotificationObject, IDisposable {
		private readonly IServerServiceFacade serverServiceFacade;

		private bool isDisposed;
		private string statusMessage = "Loading...";
		private string speechMessage;


		public MainViewModel(IServerServiceFacade serverServiceFacade) {
			if (serverServiceFacade == null)
				throw new ArgumentNullException("serverServiceFacade");

			this.serverServiceFacade = serverServiceFacade;
		}


		public async Task InitializeServices() {
			await ProviderManager.StartAsync();
		}

		public async Task InitializeConnection() {
			await this.serverServiceFacade.OpenRelayConnectionAsync();
		}

		#region Properties

		public string StatusMessage {
			get { return this.statusMessage; }
			set { 
				this.statusMessage = value ?? String.Empty;
				this.RaisePropertyChanged(() => this.StatusMessage);
			}
		}

		public string SpeechMessage {
			get { return this.speechMessage; }
			set {
				this.speechMessage = value ?? String.Empty;
				this.RaisePropertyChanged(() => this.SpeechMessage);
			}
		}

		#endregion

		#region IDisposable implementation

		public void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected async virtual void Dispose(bool disposing) {
			if (this.isDisposed) return;

			if (disposing) {
				bool closed = await this.serverServiceFacade.CloseRelayConnectionAsync();
				// TODO alert user that there could be troubles connecting to service
			}

			this.isDisposed = true;
		}

		#endregion
	}
}
