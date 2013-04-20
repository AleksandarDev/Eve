using System.ServiceModel;

namespace Eve.API.Services.Contracts.Services.Interfaces {
	[ServiceContract(CallbackContract = typeof(IRelayCallbackContract))]
	public interface IRelayService : IEveAPIService {
		[OperationContract(IsOneWay = true)]
		void Subscribe();

		[OperationContract(IsOneWay = true)]
		void Unsibscribe();

		[OperationContract]
		bool UpdateClientState(ClientState state);
	}
}
