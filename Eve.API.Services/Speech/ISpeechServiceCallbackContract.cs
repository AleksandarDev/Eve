namespace Eve.API.Services.Speech {
	public interface ISpeechServiceCallbackContract {
		void SpeechSynthesisRequested(string message);
		void SpeechSynthesisStarted(string message);
		void SpeechSynthesisCompleted(string message);
	}
}