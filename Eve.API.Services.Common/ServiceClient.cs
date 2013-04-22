using System.Runtime.Serialization;

namespace Eve.API.Services.Common {
	[DataContract]
	public class ServiceClient {
		public ServiceClient(string alias, string id) {
			this.Alias = alias;
			this.ID = id;
		}


		#region Properties

		[DataMember]
		public string Alias { get; set; }

		[DataMember]
		public string ID { get; set; }

		public object Callback { get; set; }

		#endregion
	}
}