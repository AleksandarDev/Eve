using System.ServiceModel;
using Eve.API.Services.Common;
using Eve.API.Services.Common.Modules.Touch;
using Eve.API.Services.Contracts.Modules;

namespace Eve.API.Services.Contracts {
	[ServiceContract]
	public interface IEveAPIService/* : ITouchService */{
		[OperationContract]
		bool SignIn(ServiceUser user);

		[OperationContract]
		bool SignOut(ServiceUser user);

		[OperationContract]
		ServiceClient[] GetAvailableClients();

		[OperationContract]
		bool SendTrackPadMessage(ServiceRequestDetails details, TrackPadMessage message);

		[OperationContract]
		bool SendButtonMessage(ServiceRequestDetails details, ButtonMessage message);
	}
}
