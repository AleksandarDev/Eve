using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EveControl.Adapters;
=======
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Eve.API;
using Eve.API.Chrome;
using EveControl.Adapters;
using Fleck2.Interfaces;
>>>>>>> master
using Microsoft.Practices.Prism.ViewModel;

namespace EveControl.Windows.Chrome {
	public class ChromeViewModel : NotificationObject, IDisposable {
		private readonly IServerServiceFacade serverServiceFacade;

		public ChromeViewModel(IServerServiceFacade serverServiceFacade) {
			if (serverServiceFacade == null)
				throw new ArgumentNullException("serverServiceFacade");

			this.serverServiceFacade = serverServiceFacade;
<<<<<<< HEAD
		}

=======

			this.Clients = new ObservableCollection<IWebSocketConnection>();

			ProviderManager.ChromeProvider.OnClientConnected += this.OnClientConnected;
			ProviderManager.ChromeProvider.OnClientDisconnected += this.OnClientDisconnected;
		}

		private void OnClientDisconnected(ChromeProvider provider, ChromeProviderClientEventArgs args) {
			Application.Current.Dispatcher.BeginInvoke(
				new Action(() => this.Clients.Remove(args.ClientChanged)));
		}

		private void OnClientConnected(ChromeProvider provider, ChromeProviderClientEventArgs args) {
			Application.Current.Dispatcher.BeginInvoke(
				new Action(() => this.Clients.Add(args.ClientChanged)));
		}

		public void ExecuteScript(string code) {
			ProviderManager.ChromeProvider.Push(
				ChromeViewModel.Escape(code));
		}

		private static string Escape(string text) {
			if (text == null) return String.Empty;
			return text
				.Replace('\n', ' ')
				.Replace('\t'.ToString(), "   ")
				.Replace('\r', ' ')
				.Replace('\v', ' ');
		}

		#region Properties

		public ObservableCollection<IWebSocketConnection> Clients { get; private set; }

		#endregion

>>>>>>> master
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
