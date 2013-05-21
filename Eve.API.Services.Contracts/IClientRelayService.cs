using System.ServiceModel;
using Eve.API.Services.Common;

namespace Eve.API.Services.Contracts {
	[ServiceContract(CallbackContract = typeof(IEveAPIService))]
	public interface IClientRelayService {
		[OperationContract]
		bool Subscribe(ServiceClient clientData);

		[OperationContract]
		bool Unsibscribe(ServiceClient clientData);

		[OperationContract]
		string ClientPing(string yourName);
	}
}
