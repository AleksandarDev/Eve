using System.Runtime.Serialization;
using Eve.API.Services.Contracts.Services.Interfaces;

namespace Eve.API.Services.Contracts.Services {
	[DataContract]
	public class ServiceClient {
		public ServiceClient(string alias, string id) {
			this.Alias = alias;
			this.ID = id;
		}


		#region Properties

		[DataMember]
		public string Alias { get; private set; }

		[DataMember]
		public string ID { get; private set; }

		public IRelayCallbackContract Callback { get; set; }

		#endregion
	}
}