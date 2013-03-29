using System.Runtime.Serialization;

namespace Eve.API.Services.Contracts.Services {
	[DataContract]
	public class ServiceClient {
		#region Properties

		[DataMember]
		public string Alias { get; private set; }

		[DataMember]
		public string ID { get; private set; }

		#endregion
	}
}