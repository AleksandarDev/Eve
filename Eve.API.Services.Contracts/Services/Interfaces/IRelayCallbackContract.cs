using System.ServiceModel;

namespace Eve.API.Services.Contracts.Services.Interfaces {
	public interface IRelayCallbackContract {
		[OperationContract]
		string PingRequest(string message);
	}
}