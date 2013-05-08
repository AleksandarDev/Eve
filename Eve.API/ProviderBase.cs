using System.Threading.Tasks;
using Eve.Diagnostics.Logging;

namespace Eve.API {
	public abstract class ProviderBase<T> : IProvider {
		protected readonly Log.LogInstance log =
			new Log.LogInstance(typeof(T));

		/// <summary>
		/// Starts provider and its components
		/// </summary>
		/// <returns>Returns asynchronous void Task</returns>
		public virtual async Task StartAsync() {
			// Make sure we don't start provider twice
			if (this.IsRunning) {
				this.log.Warn("Provider is already started");
				return;
			}

			this.log.Info("Starting provider...");

			await Task.Run(() => this.Initialize());

			this.IsRunning = true;
			this.log.Info("Provider started");

			if (this.OnStarted != null)
				this.OnStarted(this);
		}

		/// <summary>
		/// Stops provider and its components
		/// </summary>
		/// <returns>Returns asynchronous void Task</returns>
		public virtual async Task StopAsync() {
			this.log.Info("Stopping provider...");

			await Task.Run(() => this.Uninitialize());

			this.IsRunning = false;
			this.log.Info("Provider stopped");

			if (this.OnStopped != null)
				this.OnStopped(this);
		}

		/// <summary>
		/// Resets providers' settings
		/// </summary>
		/// <returns>Returns asynchronous void Task</returns>
		public async Task ResetAsync() {
			await this.StopAsync();
			await this.StartAsync();
		}

		/// <summary>
		/// Initializes provider components
		/// This is called upon Start call
		/// </summary>
		/// <returns>Returns asynchronous void Task</returns>
		protected abstract void Initialize();

		/// <summary>
		/// Uninitializes providers components
		/// This is called upon Stop call
		/// </summary>
		/// <returns></returns>
		protected abstract void Uninitialize();

		/// <summary>
		/// Check whether provider is running - writes warning if it's not running
		/// </summary>
		/// <returns>Returns Boolean value indicating whether provider is started</returns>
		protected virtual bool CheckIsRunning() {
			if (!this.IsRunning)
				this.log.Warn("Start provider before using it!");

			return this.IsRunning;
		}


		#region Properties

		/// <summary>
		/// Gets indication whether provider is running
		/// </summary>
		public virtual bool IsRunning { get; private set; }

		// TODO Comment event
		public event ProviderEventHandler OnStarted;
		// TODO Comment event
		public event ProviderEventHandler OnStopped;

		#endregion

		/// <summary>
		/// Stops provider on call
		/// </summary>
		public virtual async void Dispose() {
			await this.StopAsync();
		}
	}
}