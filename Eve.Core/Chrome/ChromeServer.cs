using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fleck2;
using Fleck2.Interfaces;

namespace Eve.Core.Chrome {
	public class ChromeServer {
		public const string DefaultServerLocation = "ws://localhost";
		public const int DefaultServerPort = 41258;

		private WebSocketServer server;
		private List<IWebSocketConnection> clientConnections;
		private string serverLocation = DefaultServerLocation;
		private int serverPort = DefaultServerPort;


		public ChromeServer() {
			this.InitializeAsync();
		}

		public ChromeServer(string location, int port) : this() {
			this.ServerLocation = location;
			this.ServerPort = port;
		}


		private async void InitializeAsync() {
			await this.InitializeServerAsync();
		}

		public async Task InitializeServerAsync() {
			// Clear connections
			await this.ClearConnectionsAsync();

			// Start new server
			this.server = new WebSocketServer(String.Format("{0}:{1}", this.serverLocation, this.serverPort));
			this.server.Start(conn => {
				conn.OnOpen = () => this.OnOpen(conn);
				conn.OnMessage = this.OnMessage;
				conn.OnError = this.OnError;
				conn.OnClose = () => this.OnClose(conn);
			});
		}

		private void OnClose(IWebSocketConnection connection) {
			if (!this.clientConnections.Contains(connection))
				System.Diagnostics.Debug.WriteLine("Can't close connection, connection doesn't exist", typeof (ChromeServer).Name);
			else {
				connection.Close();
				this.clientConnections.Remove(connection);
			}
		}

		private void OnError(Exception exception) {
			System.Diagnostics.Debug.WriteLine(String.Format("An error occured WebSocketServer\n{0}", exception.Message),
			                                   typeof (ChromeServer).Name);
		}

		private void OnMessage(string message) {
			System.Diagnostics.Debug.WriteLine(String.Format("Got message: {0}", message), typeof (ChromeServer).Name);
		}

		private void OnOpen(IWebSocketConnection connection) {
			if (this.clientConnections.Contains(connection)) 
				System.Diagnostics.Debug.WriteLine("Connection already exists", typeof (ChromeServer).Name);
			
			this.clientConnections.Add(connection);
		}

		public void Push(string message) {
			this.clientConnections.ForEach(conn => conn.Send(message));
		}

		public void Push(byte[] data) {
			this.clientConnections.ForEach(conn => conn.Send(data));
		}

		/// <summary>
		/// Closes and removes all connections currently registered
		/// </summary>
		public async Task ClearConnectionsAsync() {
				if (this.clientConnections == null)
					this.clientConnections = new List<IWebSocketConnection>();
				else {
					// Close all connections
					await Task.Run(() => {
						this.clientConnections.ForEach(conn => conn.Close());
					});

					// Clear connections list (all connections are closed)
					this.clientConnections.Clear();
				}
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
				else System.Diagnostics.Debug.WriteLine(String.Format("Invalid port: {0}", value), typeof (ChromeServer).Name);
			}
		}

		#endregion
	}
}
