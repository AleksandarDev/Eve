using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Eve.API;
using Eve.API.Speech;
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

			// Call initialize if ProviderManager is ready
			if (!ProviderManager.IsInitialized)
				ProviderManager.OnInitialized += this.Initialize;
			else this.Initialize();
		}

		public async void Initialize() {
			// Attach to speech provider events
			ProviderManager.SpeechProvider.OnRecognitionAccepted +=
				this.OnRecognitionAccepted;
			ProviderManager.SpeechProvider.OnRecognitionHypothesized +=
				this.OnRecognitionHypothesized;
			ProviderManager.SpeechProvider.OnRecognitionRejected +=
				this.OnRecognitionRejected;

			this.StatusMessage = "Starting service...";
			await ProviderManager.StartAsync();

			this.StatusMessage = "Connecting to relay service...";
			await this.serverServiceFacade.OpenRelayConnectionAsync();

			this.StatusMessage = "Ready";
		}

		private void OnRecognitionAccepted(SpeechProviderRecognozerEventArgs args) {
			this.SpeechMessage = args.Result.Text;

			Task.Run(() => {
				System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));
				this.SpeechMessage = String.Empty;
			});
		}

		private void OnRecognitionHypothesized(SpeechProviderRecognozerEventArgs args) {
			this.SpeechMessage = args.Result.Text;
		}

		private void OnRecognitionRejected(SpeechProviderRecognozerEventArgs args) {
			this.SpeechMessage = args.Result.Text;
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
