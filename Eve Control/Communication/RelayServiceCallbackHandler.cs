using EveControl.RelayServiceReference;

namespace EveControl.Communication {
	/// <summary>
	/// Implementation of Relay Service Callback methods
	/// </summary>
	public class RelayServiceCallbackHandler : IRelayServiceCallback {
		public string PingRequest(string message) {
			return "Control" + message;
		}
	}
}