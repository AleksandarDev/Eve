using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eve.API;
using Fleck2;
using Fleck2.Interfaces;

namespace Eve.API.Chrome {
	public class ChromeProviderClientEventArgs {
		public WebSocketConnection ClientChanged { get; private set; }
		public IEnumerable<WebSocketConnection> Clients { get; private set; }

		public ChromeProviderClientEventArgs(WebSocketConnection clientChanged, IEnumerable<WebSocketConnection> clients) {
			this.ClientChanged = clientChanged;
			this.Clients = clients;
		}
	}

	//public delegate void ChromeProviderEventHandler(
	//	ChromeProvider provider, ChromeProviderEventArgs args);

	public delegate void ChromeProviderClientEventHandler(
		ChromeProvider provider, ChromeProviderClientEventArgs args);

	[ProviderDescription("ChromeProvider")]
	public class ChromeProvider : ProviderBase<ChromeProvider> {
		public const string DefaultServerLocation = "ws://localhost";
		public const int DefaultServerPort = 41258;

		public event ChromeProviderClientEventHandler OnClientConnected;
		public event ChromeProviderClientEventHandler OnClientDisconnected;

		private WebSocketServer server;
		private List<IWebSocketConnection> clientConnections;
		private string serverLocation = DefaultServerLocation;
		private int serverPort = DefaultServerPort;


		protected override void Initialize() {
			// Set defaults
			this.ServerLocation = DefaultServerLocation;
			this.ServerPort = DefaultServerPort;

			// Clear connections
			this.ClearConnections();

			// Start new server
			this.log.Info("Starting new server on {0}:{1}",
						  this.serverLocation, this.serverPort);
			this.server = new WebSocketServer(
				String.Format("{0}:{1}", this.serverLocation, this.serverPort));
			this.server.Start(conn => {
				conn.OnOpen = () => this.OnOpen(conn);
				conn.OnMessage = this.OnMessage;
				conn.OnError = this.OnError;
				conn.OnClose = () => this.OnClose(conn);
			});
		}

		protected override void Uninitialize() {
			this.ClearConnections();
		}

		/// <summary>
		/// Closes and removes all connections currently registered
		/// </summary>
		public void ClearConnections() {
			this.log.Info("Clearing connections...");

			if (this.clientConnections == null)
				this.clientConnections = new List<IWebSocketConnection>();
			else {
				// Close all connections
				this.clientConnections.ForEach(conn => {
					this.log.Info("Closing connection to \"{0}\"",
								  conn.ConnectionInfo.ClientIpAddress);
					conn.Close();
				});

				// Clear connections list (all connections are closed)
				this.clientConnections.Clear();
			}

			this.log.Info("Connections cleared");
		}

		private void OnClose(IWebSocketConnection connection) {
			if (!this.clientConnections.Contains(connection))
				this.log.Warn("Can't close connection, connection doesn't exist", typeof (ChromeProvider).Name);
			else {
				connection.Close();
				this.clientConnections.Remove(connection);
			}
		}

		private void OnError(Exception exception) {
			this.log.Error<Exception>(exception, "An error occured WebSocketServer\n{0}",
									  exception.Message);
			// TODO Add event
		}

		private void OnMessage(string message) {
			this.log.Info("Got message: {0}", message);
			// TODO Add event
		}

		private void OnOpen(IWebSocketConnection connection) {
			if (this.clientConnections.Contains(connection))
				this.log.Warn("Connection already exists {0}:{1}",
							  connection.ConnectionInfo.ClientIpAddress,
							  connection.ConnectionInfo.ClientPort);
			
			this.clientConnections.Add(connection);

			// TODO add event
		}

		public void Push(string message) {
			this.clientConnections.ForEach(conn => conn.Send(message));
		}

		public void Push(byte[] data) {
			this.clientConnections.ForEach(conn => conn.Send(data));
		}

		public IEnumerable<IWebSocketConnection> GetClients() {
			return this.clientConnections.AsEnumerable();
		} 

		#region Properties

		/// <summary>
		/// Gets or sets server location address.
		/// </summary>
		/// <para>
		/// This property will only change local server location variable (not the actual server's location). 
		/// In order to apply new values to the server, server needs to be reset by calling server initialization method.
		/// </para>
		public string ServerLocation {
			get { return this.serverLocation; }
			set {
				if (!String.IsNullOrEmpty(value))
					this.serverLocation = value;
			}
		}

		/// <summary>
		/// Gets or sets server port.
		/// </summary>
		/// <para>
		/// This property will only change local server port variable (not the actual server's port).
		/// In order to apply new values to the server, server needs to be reset by calling server initialization method.
		/// </para>
		public int ServerPort {
			get { return this.serverPort; }
			set {
				if (value >= 0)
					this.serverPort = value;
				else System.Diagnostics.Debug.WriteLine(String.Format("Invalid port: {0}", value), typeof (ChromeProvider).Name);
			}
		}

		#endregion
	}
}
