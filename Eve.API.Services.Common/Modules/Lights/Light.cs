using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Eve.API.Services.Common.Modules.Lights {
	[DataContract(Name = "Light")]
	public class Light {
		public Light(int id, string alias, bool state) {
			this.ID = id;
			this.Alias = alias ?? id.ToString();
			this.State = state;
		}

		#region Properties

		[DataMember]
		public int ID { get; set; }

		[DataMember]
		public string Alias { get; set; }

		[DataMember]
		public bool State { get; set; }

		#endregion

		public override string ToString() {
			return String.Format("{0}: {1}", this.Alias, this.State ? "On" : "Off");
		}
	}
}
