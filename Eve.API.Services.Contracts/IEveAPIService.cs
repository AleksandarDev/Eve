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
		bool SendTrackPadMessage(string client, TrackPadMessage message);

		[OperationContract]
		bool SendButtonMessage(string client, ButtonMessage message);

		#endregion

		#region DisplayEnhancement implementation

		[OperationContract]
		bool SetZoom(string client, int zoomValue);

		#endregion

		#region Lights implementation

		[OperationContract]
		Light[] GetLights(string client);

		[OperationContract]
		bool SetLightState(string client, int id, bool state);

		#endregion

		#region Ambiental implementation

		[OperationContract]
		AmbientalLight[] GetAmbientalLights(string client);

		[OperationContract]
		bool SetAmbientalLightState(string client, int id, bool state);

		[OperationContract]
		bool SetAmbientalLightColor(string client, int id, byte r, byte g, byte b, byte a);

		#endregion
	}
}
