using System.ServiceModel;

namespace Eve.API.Services.Contracts.Services.Interfaces {
	[ServiceContract(CallbackContract = typeof(IRelayCallbackContract))]
	public interface IRelayService : IEveService {
		[OperationContract]
		string Ping(string yourName);

		[OperationContract(IsOneWay = true)]
		void PingOneWay(string yourName);

		[OperationContract(IsOneWay = true)]
		void Subscribe();

		[OperationContract(IsOneWay = true)]
		void Unsibscribe();

		[OperationContract]
		ServiceClient[] GetAvailableClients();

		[OperationContract]
		void GetClientState();

		[OperationContract]
		bool UpdateClientState();
	}
}
