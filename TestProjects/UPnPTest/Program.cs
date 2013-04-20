using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ManagedUPnP;
using ManagedUPnP.Descriptions;

namespace UPnPTest {
	internal class Program {
		private const string onIp = "192.168.1.4";
		private const int onPort = 46610;
		private const string onPortProtocol = "tcp";

		private static void Main(string[] args) {
			ServiceHost host = new ServiceHost(typeof(Eve.API.Services.Relay.RemoteRelayService),
											   new Uri("http://" + onIp + "/", UriKind.Absolute));
			Console.WriteLine("Opening host...");
			host.Open(TimeSpan.FromSeconds(10));
			Console.WriteLine("Host opened");

			Service service = null;
			Services services = Discovery.FindServices("urn:schemas-upnp-org:service:WANPPPConnection:1");
			if (services.Count > 0)
				service = services[0];
			else Console.WriteLine("No WAN devices found...");

			if (service != null)
				try {
					var description = service.Description();
					if (description.Actions.ContainsKey("AddPortMapping"))
						Console.WriteLine("WAN device supports Port Mapping");
					else throw new InvalidOperationException("Port mapping not supported");

					// Remove port mapping if exists (we'll create fresh one later)
					try {
						service.InvokeAction("DeletePortMapping",
											 new object[] { "", onPort, onPortProtocol });
					}
					catch (Exception) {
						Console.WriteLine("Can't remove " + onPortProtocol + " port mapping for port " + onPort);
					}

					// Add the port mapping
					var loObj = new object[] { "", onPort, onPortProtocol, onPort, onIp, true, "EveServer", 0 };
					var result = service.InvokeAction("AddPortMapping", loObj);

					Console.WriteLine("Port mapped\nPress any key to remove port mapping...");
					Console.ReadKey();

					// Remove the port mapping
					loObj = new object[] { "", onPort, onPortProtocol };
					service.InvokeAction("DeletePortMapping", loObj);

					Console.WriteLine("Port removed");
				}
				catch (Exception loE) {
					Console.WriteLine(
						String.Format(
							"{0}: HTTPSTATUS: {1}",
							loE.Message,
							service.LastTransportStatus));
				}
			else Console.WriteLine("Service is null");

			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}
	}
}
