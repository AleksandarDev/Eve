using System.ServiceModel;
using Eve.API.Services.Common;
using Eve.API.Services.Common.Modules.Ambiental;
using Eve.API.Services.Common.Modules.Lights;
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

		#region Touch implementation

		[OperationContract]
		bool SendTrackPadMessage(ServiceRequestDetails details, TrackPadMessage message);

		[OperationContract]
		bool SendButtonMessage(ServiceRequestDetails details, ButtonMessage message);

		#endregion

		#region DisplayEnhancement implementation

		[OperationContract]
		bool SetZoom(ServiceRequestDetails details, int zoomValue);

		#endregion

		#region Lights implementation

		[OperationContract]
		Light[] GetLights();

		//[OperationContract]
		//bool SetLightState(int id, bool state);

		#endregion

		#region Ambiental implementation

		[OperationContract]
		AmbientalLight[] GetAmbientalLights();

		//[OperationContract]
		//bool SetAmbientalLightState(int id, bool state);

		//[OperationContract]
		//bool SetAmbientalLightColor(byte r, byte g, byte b, byte a);

		#endregion
	}
}
