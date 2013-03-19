using System;
using System.ServiceModel;
using Eve.API.Speech;
using Eve.Core;

namespace Eve.API.Services.Speech {
	[ServiceBehavior(
		InstanceContextMode = InstanceContextMode.PerSession, 
		ConcurrencyMode = ConcurrencyMode.Reentrant)]
	public sealed class SpeechService : ISpeechService {
		private ServiceUser sessionUser;
		private bool IsUserValidated { get { return this.sessionUser != null; } }


		public SpeechService() {
			System.Diagnostics.Debug.WriteLine(String.Format("Speech Service object created [{0}]", this.GetHashCode() ));

			if (!SpeechProvider.IsRunning)
				throw new EveException("Initialize SpeechProvider before starting service!");
		}

		~SpeechService() {
			System.Diagnostics.Debug.WriteLine(String.Format("Speech Service object destroyed [{0}]", this.GetHashCode()));
		}


		public void Speak(string message) {
			if (!IsUserValidated)
				// TODO Return exception User not validated
				return;
			System.Diagnostics.Debug.WriteLine("Speek method called!");
			SpeechProvider.Speak(new SpeechPrompt(message));
		}

		public void ValidateUser(ServiceUser user) {
			System.Diagnostics.Debug.WriteLine("Validating user: " + user.Name);

			// TODO Implement validating 
			this.sessionUser = user;
		}
	}
}
