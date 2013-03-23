using System;
using System.ServiceModel;
using System.Threading.Tasks;
using Eve.API.Speech;
using Eve.Core;

namespace Eve.API.Services.Speech {
	[ServiceBehavior(
		InstanceContextMode = InstanceContextMode.Single, 
		ConcurrencyMode = ConcurrencyMode.Multiple)]
	public sealed class SpeechService : ISpeechService {
		public SpeechService() {
			System.Diagnostics.Debug.WriteLine(String.Format("Speech Service object created [{0}]", this.GetHashCode() ));

			if (!SpeechProvider.IsRunning)
				Task.Run(async delegate { await SpeechProvider.Start(); });
		}

		~SpeechService() {
			System.Diagnostics.Debug.WriteLine(String.Format("Speech Service object destroyed [{0}]", this.GetHashCode()));
		}


		public void Speak(string token, string message) {
			throw new NotImplementedException();
		}

		public string SignIn(string userName, string passwordHash) {
			throw new NotImplementedException();
		}

		public void SignOut(string token) {
			throw new NotImplementedException();
		}
	}
}
