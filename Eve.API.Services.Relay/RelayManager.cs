using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eve.API.Services.Contracts.Services;

namespace Eve.API.Services.Relay {
	/// <summary>
	/// Static class used for communication and storing data 
	/// between Client and Remote services
	/// </summary>
	public static class RelayManager {
		/// <summary>
		/// Dictionary of service clients saved by client ID as a key
		/// </summary>
		private readonly static Dictionary<string, ServiceClient> Clients = 
			new Dictionary<string, ServiceClient>();


		/// <summary>
		/// Registers new Service Client
		/// </summary>
		/// <param name="client">Service Client object to register</param>
		/// <returns>
		/// Returns true if Service Client isn't 
		/// already registered nor null
		/// </returns>
		public static bool RegisterClient(ServiceClient client) {
			if (client == null || 
				RelayManager.Clients.ContainsKey(client.ID))
				return false;

			RelayManager.Clients.Add(client.ID, client);
			return true;
		}

		/// <summary>
		/// Unregisters already registered Service Client
		/// </summary>
		/// <param name="clientID">ID of the Service Client to unregister</param>
		/// <returns>
		/// Returns true if Service Client is registered
		/// </returns>
		public static bool UnregisterClient(string clientID) {
			if (String.IsNullOrEmpty(clientID) ||
				!RelayManager.Clients.ContainsKey(clientID)) 
				return false;

			return RelayManager.Clients.Remove(clientID);
		}

		/// <summary>
		/// Retrieve Service Client of given ID
		/// </summary>
		/// <param name="clientID">Service Client ID to retrieve</param>
		/// <returns>
		/// Returns ServiceClient object of given ID, 
		/// null if isn't registered
		/// </returns>
		public static ServiceClient GetClient(string clientID) {
			if (String.IsNullOrEmpty(clientID))
				throw new ArgumentNullException("clientID");

			ServiceClient client = null;
			var clientExists = RelayManager.Clients.TryGetValue(clientID, out client);
			return clientExists ? client : null;
		}

		/// <summary>
		/// Retrieves all registered Service Clients
		/// </summary>
		/// <returns>Enumerable list of all registered Service Clients</returns>
		public static IEnumerable<ServiceClient> GetClients() {
			return RelayManager.Clients.Values.AsEnumerable();
		} 
	}
}