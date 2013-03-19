using System;
using Microsoft.Speech.Synthesis;

namespace Eve.API.Speech {
	public class SpeechProviderSynthesizerEventArgs : EventArgs {
		public SpeechPrompt Prompt { get; set; }
		public SpeechSynthesizer Synthesizer { get; set; }
	}
}