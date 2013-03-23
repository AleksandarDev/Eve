using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Eve.API.Services;
using Eve.API.Services.TestConsole.TouchServiceReference;

namespace Eve.API.Services.TestConsole {
	class Program {
		static void Main(string[] args) {
			//EndpointAddress endpoint = new EndpointAddress(new Uri("http://localhost:41250/API/Speech/"));
			//ISpeechService proxy = DuplexChannelFactory<ISpeechService>.CreateChannel(new WSHttpBinding(), endpoint);
			//proxy.Speak("Hello");
			//Console.WriteLine("Request sent!");

			//SpeechServiceClient client = new SpeechServiceClient();
			//client.Open();
			//client.Speak("Hello");
			//client.Close();

			TouchServiceClient client = new TouchServiceClient();
			System.Diagnostics.Debug.WriteLine("Opening...");
			client.Open();
			System.Diagnostics.Debug.WriteLine("Opened");
			client.MoveMouse("", 400, 400);
			System.Diagnostics.Debug.WriteLine("Closing...");
			client.Close();
			System.Diagnostics.Debug.WriteLine("Closed");
		}
	}
}
