using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Eve.API.Services.Relay.Hosting.Console {
	public class Program {
		static void Main(string[] args) {
			System.Console.WriteLine("Creating host...");
			var host = new ServiceHost(typeof(Eve.API.Services.Relay.RelayService),
									   new Uri("http://192.168.1.101/eve/relay/",
											   UriKind.RelativeOrAbsolute));
			host.Description.Endpoints.Add(new ServiceEndpoint(
				new ContractDescription("")));
			host.Open();
			System.Console.WriteLine("Hosting initiated");

			while (host.State == CommunicationState.Opened) {
				
			}
		}
	}
}
