using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace EveInternetManager {
	// TODO Implement user identification for host registration

	[ServiceContract(SessionMode = SessionMode.Allowed)]
	public interface IEveInternetManagerService {
		[OperationContract]
		bool RegisterHost(HostDescription description);

		[OperationContract]
		HostDescription GetHostInfo(string id);
	}

	[DataContract]
	public class HostDescription {
		public string Alias { get; set; }
		public string ID { get; set; }
		public string Address { get; set; }
	}
}
