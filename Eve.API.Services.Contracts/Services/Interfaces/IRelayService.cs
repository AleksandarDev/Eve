using System.ServiceModel;

namespace Eve.API.Services.Contracts.Services.Interfaces {
	[ServiceContract(CallbackContract = typeof(IRelayCallbackContract))]
	public interface IRelayService {
		[OperationContract(IsOneWay = true)]
		void Subscribe();

		[OperationContract(IsOneWay = true)]
		void Unsibscribe();

		[OperationContract(IsOneWay = true)]
		void UpdateClientState(ClientState state);
	}
}
