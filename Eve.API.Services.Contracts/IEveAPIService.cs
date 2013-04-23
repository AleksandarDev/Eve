using System.ServiceModel;
using Eve.API.Services.Common;
using Eve.API.Services.Common.Modules.Touch;

namespace Eve.API.Services.Contracts {
	[ServiceContract]
	public interface IEveAPIService {
		[OperationContract]
		bool SignIn(ServiceUser user);

		[OperationContract]
		bool SignOut(ServiceUser user);

		[OperationContract]
		ServiceClient[] GetAvailableClients();

		#region Touch module implementation

		[OperationContract]
		bool SendTrackPadMessage(ServiceRequestDetails details, TrackPadMessage message);

		[OperationContract]
		bool SendButtonMessage(ServiceRequestDetails details, ButtonMessage message);

		#endregion
	}
}
