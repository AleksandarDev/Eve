using System;
using System.Speech.Synthesis;

namespace Eve.API.Speech {
	public class SpeechPrompt {
		private string promptMessage;
		private Prompt prompt;


		#region Constructors/Destructors

		public SpeechPrompt(string message) {
			this.SetMessage(message);
		}

		public SpeechPrompt() : this(String.Empty) {}

		#endregion


		public void SetMessage(string message) {
			this.promptMessage = message ?? String.Empty;
			this.prompt = new Prompt(this.promptMessage);
		}

		public static explicit operator Prompt(SpeechPrompt value) {
			return value.Prompt;
		}

		public override int GetHashCode() {
			return this.Message.GetHashCode();
		}

		public override bool Equals(object obj) {
			var promptObj = obj as SpeechPrompt;
			if (promptObj == null) return false;
			return promptObj.Message == this.Message;
		}

		#region Properties

		public Prompt Prompt {
			get { return this.prompt; }
		}

		public string Message {
			get { return this.promptMessage;}
			set { this.SetMessage(value); }
		}

		#endregion
	}
}