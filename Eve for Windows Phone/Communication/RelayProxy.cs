using System;
using System.ServiceModel;
using System.Threading;
using Eve.Diagnostics.Logging;
using EveWindowsPhone.RelayServiceReference;

namespace EveWindowsPhone.Communication {
	/// <summary>
	/// Helper class to handle relay connection and calls
	/// </summary>
	public class RelayProxy : IDisposable {
		// TODO Add firewall exception on initial connection timed out http://msdn.microsoft.com/en-us/library/aa366421(VS.85).aspx
		// TODO Implement user signin/signout
		// TODO Implement client choosing
		// TODO Implement Request details change/collection
		protected readonly Log.LogInstance log =
			new Log.LogInstance(typeof(RelayProxy));

		protected bool isConnected;
		//protected readonly Timer timer;
		//protected const int ConnectionCheckInterval = 5000;
		//protected const string PingRequestContent = "Client";
		//protected const string PingResponsePrefix = "Hello ";

		public event RelayProxyEventHandler OnOpened;
		public event RelayProxyEventHandler OnClosed;
		public event RelayProxyEventHandler ConnectionChanged;


		/// <summary>
		/// Object constructor
		/// </summary>
		public RelayProxy() {
			// Create relay proxy
			this.Relay = new EveAPIServiceClient();

			// Set connection to false (this raises events)
			this.IsConnected = false;

			// TODO Remove, only for testing
			this.ActiveClient = "AleksandarPC";

			// Set periodic connection tests
			//this.timer = new Timer(ConnectionCheckInterval);
			//this.timer.Elapsed += async (s, a) => await this.CheckConnection();
		}


		/// <summary>
		/// Opens connection to relay if already not opened
		/// </summary>
		public void Open() {
			// Check if communication isn't opening already
			if (this.Relay.State == CommunicationState.Opening) {
				this.log.Info("Already opening...");
			}

			// Check if connection is in faulted state so that we
			// abort all operations on proxy and get it to closed state
			if (this.Relay.State == CommunicationState.Faulted) {
				this.log.Info("Connection is faulted! Aborting and restarting.");
				this.Relay.Abort();
			}

			// Open connection to relay
			try {
				this.log.Info("Opening connection...");
				this.Relay.OpenAsync();
				this.Relay.OpenCompleted += (sender, args) => {
					this.log.Info("Connection opened");
					this.IsConnected = true;
				};

				// Start connection check timer
				//this.timer.Start();
			}
			catch (TimeoutException ex) {
				this.log.Error<TimeoutException>(ex,
												 "Unable to open connection to proxy - timed out");
				this.IsConnected = false;
				// TODO Activate pooling
				// NOTE This could be due to firewall
			}
			catch (Exception ex) {
				this.log.Error<Exception>(ex,
										  "Unknown error occurred while connecting to proxy");
				this.IsConnected = false;
			}
		}

		/// <summary>
		/// Closes connection to relay if already not closed
		/// </summary>
		/// <param name="forceClose">Set this to true if connection should be aborted without waiting for any calls to finish</param>
		protected virtual void Close(bool forceClose = false) {
			// Check if connection isn't already closed
			if (this.Relay.State == CommunicationState.Closed) {
				this.log.Info("Connection already closed");
			}

			// Try to close connection
			try {
				if (forceClose) {
					this.log.Info("Aborting connection...");
					this.Relay.Abort();
					this.IsConnected = false;
					this.log.Info("Connection aborted");
				}
				else {
					this.log.Info("Closing connection...");
					this.Relay.CloseAsync();
					this.Relay.CloseCompleted += (s, a) => {
						this.log.Info("Connection closed");
						this.IsConnected = false;
					};
				}
			}
			catch (Exception ex) {
				this.log.Error<Exception>(ex, "Unable close/abort connection!");
			}
		}

		// TODO Implement connection check (add Ping to eve contract)
		///// <summary>
		///// Checks if relay service still responds to API calls by sending Ping request
		///// </summary>
		///// <returns>Returns Boolean value indicating whether connection is confirmed</returns>
		//protected virtual bool CheckConnection() {
		//	this.log.Info("Checking connection...");

		//	// Check if connection needs to be established first
		//	if (this.Relay.State != CommunicationState.Opened) {
		//		this.log.Warn("Connection closed. Requesting to reopen...");
		//		this.IsConnected = false;
		//		return this.Open();
		//	}

		//	// Call Ping to relay service and test if result is valid
		//	try {
		//		string response = await this.Relay.(PingRequestContent);
		//		if (response != PingResponsePrefix + PingRequestContent) {
		//			this.log.Warn("Ping responded with wrong data");
		//			this.IsConnected = false;
		//		} else {
		//			this.log.Info("Connection confirmed");
		//			this.IsConnected = true;
		//		}
		//	} catch (Exception) {
		//		this.log.Warn("Connection to relay timed out");
		//		this.IsConnected = false;
		//	}

		//	return this.IsConnected;
		//}

		// CLose connection to relay
		public void Dispose() {
			this.Close(true);
		}

		#region Properties

		/// <summary>
		/// Gets Relay client from which API calls can be made
		/// </summary>
		public EveAPIServiceClient Relay { get; private set; }

		/// <summary>
		/// Gets currently active client ID
		/// </summary>
		public string ActiveClient { get; private set; }

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
	}
}