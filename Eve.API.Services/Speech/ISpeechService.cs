using System.Net.Security;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Eve.API.Services.Speech {
	[ServiceContract(SessionMode = SessionMode.Allowed)]
	public interface ISpeechService {
		[OperationContract]
		void Speak(string token, string message);

		[OperationContract]
		string SignIn(string userName, string passwordHash);

		[OperationContract]
		void SignOut(string token);
	}
}