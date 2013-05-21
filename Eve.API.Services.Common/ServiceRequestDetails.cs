using System.Runtime.Serialization;

namespace Eve.API.Services.Common {
	[DataContract]
	public class ServiceRequestDetails {
		#region Properties

		[DataMember]
		public ServiceClient Client { get; set; }

		[DataMember]
		public ServiceUser User { get; set; }

		#endregion
	}
}