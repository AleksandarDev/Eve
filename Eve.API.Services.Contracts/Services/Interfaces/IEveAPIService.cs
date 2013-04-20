using System.ServiceModel;

namespace Eve.API.Services.Contracts.Services.Interfaces {
	[ServiceContract]
	public interface IEveAPIService : ITouchService {
		[OperationContract]
		string Ping(string yourName);

		[OperationContract]
		bool SignIn(ServiceUser user);

		[OperationContract]
		bool SignOut(ServiceUser user);

		[OperationContract]
		ClientState GetClientState();

		[OperationContract]
		ServiceClient[] GetAvailableClients();
	}
}
