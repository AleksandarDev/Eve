using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Eve.API.Services.Common;
using Eve.API.Services.Speech;

namespace Eve.API.Services.Touch {
	[ServiceContract(SessionMode = SessionMode.Allowed)]
	public interface ITouchService {
		[OperationContract]
		bool ProcessTrackPadMessage(string token, TrackPadMessage message);

		[OperationContract]
		bool ProcessButtonMessage(string token, ButtonMessage message);

		[OperationContract]
		string SignIn(string userName, string passwordHash);

		[OperationContract]
		void SignOut(string token);
	}
}
