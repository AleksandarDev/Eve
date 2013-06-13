using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms.VisualStyles;
using System.Windows.Navigation;
using Eve.Diagnostics.Logging;
using EveControl.RelayServiceReference;

namespace EveControl.Communication {
	/// <summary>
	/// Helper class to handle relay connection and calls
	/// </summary>
	public class RelayProxy {
		// TODO Add firewall exception on initial connection timed out http://msdn.microsoft.com/en-us/library/aa366421(VS.85).aspx
		// TODO IsSubscribed
		// TODO LocalProxy
		protected readonly Log.LogInstance log = new Log.LogInstance(typeof(RelayProxy));

		protected readonly InstanceContext instanceContext;
		protected bool isConnected;
		protected readonly Timer timer;
		protected const int ConnectionCheckInterval = 5000;
		protected const string PingRequestContent = "Client";
		protected const string PingResponsePrefix = "Hello ";

		public event RelayProxyEventHandler OnOpened;
		public event RelayProxyEventHandler OnClosed;
		public event RelayProxyEventHandler ConnectionChanged;


		/// <summary>
		/// Object constructor
		/// </summary>
		/// <param name="callbackImplementation">Relay Service callback handler object</param>
		public RelayProxy(IClientRelayServiceCallback callbackImplementation) {
			if (callbackImplementation == null)
				throw new ArgumentNullException("callbackImplementation");

			// Create relay proxy and context
			this.instanceContext = new InstanceContext(callbackImplementation);
			this.Relay = new ClientRelayServiceClient(this.instanceContext);

			// Set connection to false (this raises events)
			this.IsConnected = false;

			// Set periodic connection tests
			this.timer = new Timer(ConnectionCheckInterval);
			this.timer.Elapsed += async (s, a) => await this.CheckConnection();
		}


		/// <summary>
		/// Opens connection to relay if already not opened
		/// </summary>
		/// <returns>Returns Boolean value that indicated whether connection is successfully opened</returns>
		public async Task<bool> OpenAsync() {
			// Check if communication isn't opening already
			if (this.Relay.State == CommunicationState.Opening) {
				this.log.Info("Already opening...");
				return true;
			}

			// Check if connection is in faulted state so that we
			// abort all operations on proxy and get it to closed state
			if (this.Relay.State == CommunicationState.Faulted) {
				this.log.Info("Connection is faulted! Aborting and restarting.");
				await Task.Run(() => this.Relay.Abort());
			}

			// OpenAsync connection to relay
			this.log.Info("Opening connection...");
			await Task.Run(() => {
				try {
					this.Relay.Open();

					if (this.Relay.State == CommunicationState.Opened) {
						this.log.Info("Connection opened");
						this.IsConnected = true;

						// Start connection check timer
						this.timer.Start();
					}
				}
				catch (TimeoutException ex) {
					this.log.Error<TimeoutException>(ex,
						"Unable to open connection to proxy - timed out");
					this.IsConnected = false;
					// TODO Activate pooling
					// NOTE This could be due to firewall
				}
				catch (CommunicationObjectAbortedException ex) {
					this.log.Error<CommunicationObjectAbortedException>(ex,
						"Couldn't open connection because it was aborted already");
					this.isConnected = false;
				} catch (Exception ex) {
					this.log.Error<Exception>(ex,
						"Unknown error occurred while connecting to proxy");
					this.IsConnected = false;
				}
			});

			return this.IsConnected;
		}

		/// <summary>
		/// Closes connection to relay if already not closed asynchronously 
		/// </summary>
		/// <param name="forceClose">Set this to true if connection should be aborted without waiting for any calls to finish</param>
		/// <returns>Returns Boolean value that indicates whether connection was successfully closed</returns>
		public virtual async Task<bool> CloseAsync(bool forceClose = false) {
			return await Task.Run(() => this.Close(forceClose));
		}

		/// <summary>
		/// Closes connection to relay if already not closed
		/// </summary>
		/// <param name="forceClose">Set this to true if connection should be aborted without waiting for any calls to finish</param>
		/// <returns>Returns Boolean value that indicates whether connection was successfully closed</returns>
		public virtual bool Close(bool forceClose = false) {
			// Check if connection isn't already closed
			if (this.Relay.State == CommunicationState.Closed) {
				this.log.Info("Connection already closed");
				return true;
			}

			// Try to close connection
			try {
				if (forceClose) {
					this.log.Info("Aborting connection...");
					this.Relay.Abort();
					this.log.Info("Connection aborted");
				} else {
					this.log.Info("Closing connection...");
					this.Relay.Close();
					this.log.Info("Connection closed");
				}

				this.timer.Stop();
				return true;
			} catch (Exception ex) {
				this.log.Error<Exception>(ex, "Unable close/abort connection!");
				this.timer.Stop();
				return false;
			}
		}

		/// <summary>
		/// Checks if relay service still responds to API calls by sending Ping request
		/// </summary>
		/// <returns>Returns Boolean value indicating whether connection is confirmed</returns>
		protected virtual async Task<bool> CheckConnection() {
			this.log.Info("Checking connection...");

			// Check if connection needs to be established first
			if (this.Relay.State == CommunicationState.Closed) {
				this.log.Warn("Connection closed. Requesting to reopen...");
				this.IsConnected = false;
				return await this.OpenAsync();
			}

			// Call Ping to relay service and test if result is valid
			try {
				string response = await this.Relay.ClientPingAsync(PingRequestContent);
				if (response != PingResponsePrefix + PingRequestContent) {
					this.log.Warn("Ping responded with wrong data");
					this.IsConnected = false;
				} else {
					this.log.Info("Connection confirmed");
					this.IsConnected = true;
				}
			} catch (Exception) {
				this.log.Warn("Connection to relay timed out");
				this.IsConnected = false;
			}

			return this.IsConnected;
		}

		#region Properties

		/// <summary>
		/// Gets Relay client from which API calls can be made
		/// </summary>
		public ClientRelayServiceClient Relay { get; private set; }

		/// <summary>
		/// Gets connection status of relays service proxy
		/// When this property is set, events are called
		/// </summary>
		public virtual bool IsConnected {
			get { return this.isConnected; }
			protected set {
				bool hasChanged = this.isConnected != value;
				this.isConnected = value;

				if (hasChanged && value) {
					if (this.OnOpened != null)
						this.OnOpened(this);
				} else if (hasChanged) {
					if (this.OnClosed != null)
						this.OnClosed(this);
				}

				if (hasChanged && this.ConnectionChanged != null)
					this.ConnectionChanged(this);
			}
		}

		#endregion

		public async Task<bool> UnsubscribeAsync(ServiceClient client) {
			return await this.Relay.UnsibscribeAsync(client);
		}

		public async Task<bool> SubscribeAsync(ServiceClient client) {
			return await this.Relay.SubscribeAsync(client);
		}
	}
}