using System.ServiceModel;

namespace Eve.API.Services.Contracts.Services.Interfaces {
	[ServiceContract]
	public interface IEveService : ITouchService {
		[OperationContract]
		bool SignIn(ServiceUser user);

		[OperationContract]
		bool SignOut(ServiceUser user);
	}
}
