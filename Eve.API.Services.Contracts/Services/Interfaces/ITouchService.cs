using System.ServiceModel;

namespace Eve.API.Services.Contracts.Services.Interfaces {
	[ServiceContract]
	public interface ITouchService {
		[OperationContract]
		bool SendTrackPadMessage(ServiceRequestDetails details, TrackPadMessage message);

		[OperationContract]
		bool SendButtonMessage(ServiceRequestDetails details, ButtonMessage message);
	}
}