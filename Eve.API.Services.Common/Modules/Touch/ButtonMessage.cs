using System;
using System.Runtime.Serialization;

namespace Eve.API.Services.Common.Modules.Touch {
	[DataContract(Name = "ButtonMessage")]
	public class ButtonMessage {
		public ButtonMessage(Buttons button, ButtonCommands command) {
			this.Command = command;
			this.Button = button;
		}


		public override string ToString() {
			return String.Format("Button: {0}  |  Command: {1}", this.Button, this.Command);
		}

		#region Properties

		[DataMember]
		public ButtonCommands Command { get; set; }

		[DataMember]
		public Buttons Button { get; set; }

		#endregion


		[Flags]
		public enum ButtonCommands : uint {
			Tap = 0x1,
			Hold = 0x2,
			DoubleTap = 0x4
		}

		[Flags]
		public enum Buttons : uint {
			Left = 0x1,
			Right = 0x2,
			Middle = 0x3
		}
	}
}