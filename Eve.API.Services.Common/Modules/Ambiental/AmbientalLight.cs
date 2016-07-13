using System;
using System.Runtime.Serialization;

namespace Eve.API.Services.Common.Modules.Ambiental {
	[DataContract(Name = "AmbientalLight")]
	public class AmbientalLight : Lights.Light {
		public AmbientalLight(int id, string alias, bool state,
							  byte r, byte g, byte b, byte a)
			: base(id, alias, state) {
			this.RValue = r;
			this.GValue = g;
			this.BValue = b;
			this.AValue = a;
		}


		#region Properties

		[DataMember]
		public byte RValue { get; set; }

		[DataMember]
		public byte GValue { get; set; }

		[DataMember]
		public byte BValue { get; set; }

		[DataMember]
		public byte AValue { get; set; }

		#endregion

		public override string ToString() {
			return String.Format("{0} [{1}] RGB:{2:X}{3:X}{4:X} A:{5:X}",
								 this.Alias, this.State ? "On" : "Off",
								 this.RValue, this.GValue, this.GValue, this.AValue);
		}
	}
}
