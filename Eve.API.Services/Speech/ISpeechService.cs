using System.Net.Security;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Eve.API.Services.Speech {
	[ServiceContract(
		SessionMode = SessionMode.Required,
		CallbackContract = typeof(ISpeechServiceCallbackContract),
		ProtectionLevel = ProtectionLevel.EncryptAndSign)]
	public interface ISpeechService {
		[OperationContract(IsOneWay = true)]
		void Speak(string message);

		[OperationContract(IsOneWay = true)]
		void ValidateUser(ServiceUser user);
	}

	[DataContract]
	public class ServiceUser {
		public string Name;
	}
}