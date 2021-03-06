﻿using System;
using System.Speech.Recognition;

namespace Eve.API.Speech {
	public class SpeechProviderRecognozerEventArgs : EventArgs {
		public RecognitionResult Result { get; set; }
		public SpeechRecognitionEngine Recognizer { get; set; }
	}
}