using System;
using System.Threading.Tasks;

namespace Eve.API {
	public interface IProvider : IDisposable {
		bool IsRunning { get; }

		event ProviderEventHandler OnStarted;
		event ProviderEventHandler OnStopped;

		Task StartAsync();
		Task StopAsync();
		Task ResetAsync();
	}
}
