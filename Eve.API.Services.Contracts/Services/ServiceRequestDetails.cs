using System.Runtime.Serialization;

namespace Eve.API.Services.Contracts.Services {
	[DataContract]
	public class ServiceRequestDetails {
		#region Properties

		[DataMember]
		public ServiceClient Client { get; private set; }

		[DataMember]
		public ServiceUser User { get; private set; }

		#endregion
	}
}