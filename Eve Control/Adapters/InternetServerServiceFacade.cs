using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Eve.Diagnostics.Logging;
using EveControl.Communication;
using EveControl.RelayServiceReference;

namespace EveControl.Adapters {
	public class InternetServerServiceFacade : IServerServiceFacade {
		private readonly Log.LogInstance log =
			new Log.LogInstance(typeof(InternetServerServiceFacade));
		private readonly IServerCallbackServiceFacade serverCallbackServiceFacade;


		public InternetServerServiceFacade(IServerCallbackServiceFacade serverCallbackServiceFacade) {
			if (serverCallbackServiceFacade == null)
				throw new ArgumentNullException("serverCallbackServiceFacade");
			this.serverCallbackServiceFacade = serverCallbackServiceFacade;

			// TODO Load client data
			this.Client = new ServiceClient() {
				Alias = "Aleksandar Toplek's Laptop",
				ID = "AleksandarPC"
			};

			// Relay service
			this.RelayProxy = new RelayProxy(this.serverCallbackServiceFacade);
		}


		#region IServerServiceFacade implementation

		public ServiceClient Client { get; private set; }
		public RelayProxy RelayProxy { get; private set; }

		public async Task<bool> OpenRelayConnectionAsync() {
			// Check if proxy is valid
			if (!this.CheckRelayProxyInstantiated()) return false;

			// Try to open connection
			this.log.Info("Opening connection to relay service...");
			bool opened = await this.RelayProxy.OpenAsync();
			if (!opened) {
				this.log.Warn("Unable to open connection to relay service");
				return false;
			}
			this.log.Info("Connection to relay service opened");

			// Subscribe to service
			this.log.Info("Subscribing to relay service as {0} [{1}]...", 
				this.Client.Alias, this.Client.ID);
			bool subscribed = await this.RelayProxy.SubscribeAsync(this.Client);
			if (!subscribed) {
				this.log.Warn("Unable to subscribe to relay service!");
				return false;
			}
			this.log.Info("Subscribed to relay service!");

			return true;
		}

		public async Task<bool> CloseRelayConnectionAsync() {
			// Check if proxy is valid
			if (!this.CheckRelayProxyInstantiated()) return false;

			// Unsubscribe if need
			this.log.Info("Unsubscribing from relay service as {0} [{1}]...",
				this.Client.Alias, this.Client.ID);
			bool unsubscribed = await this.RelayProxy.UnsubscribeAsync(this.Client);
			if (!unsubscribed) {
				this.log.Warn("Unable to unsubscribe from relay service!");
				return false;
			}
			this.log.Info("Unsubscribed from relay service");

			// Try to close service connection
			this.log.Info("Closing connection to relay service...");
			bool closed = await this.RelayProxy.CloseAsync();
			if (closed) this.log.Info("Connection to relay service succsssfully closed");
			else {
				// Force connection close
				this.log.Info("Forced close in progress...");
				closed = await this.RelayProxy.CloseAsync(true);
				if (closed) this.log.Info("Connection closed by force");
				else {
					this.log.Error<Exception>(
						new Exception("RelayProxy"),
						"Couldn't close connection to relay service by force!");
					return false;
				}
			}

			return true;
		}

		#endregion

		private bool CheckRelayProxyInstantiated() {
			if (this.RelayProxy == null) {
				this.log.Warn("Couldn't close connection, relay proxy is not instantiated yet");
				return false;
			}
			return true;
		}
	}
}
