using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eve.API {
	[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
	public class ProviderDescription : Attribute {
		public string Name { get; private set; }
		public Type[] Dependencies { get; private set; }

		/// <summary>
		/// Object constructor
		/// </summary>
		/// <param name="name">Name (Alias) of provider</param>
		/// <param name="dependencies">Dependencies that need to be initialized</param>
		public ProviderDescription(string name, params Type[] dependencies) {
			this.Name = name;
			this.Dependencies = dependencies;

			if (dependencies.Any(d => !d.IsAssignableFrom(typeof(IProvider))))
				throw new InvalidCastException("Some dependencies aren't of required type");
		}
	}

	public interface IProvider : IDisposable {
		bool IsRunning { get; }

		event ProviderEventHandler OnStarted;
		event ProviderEventHandler OnStopped;

		Task StartAsync();
		Task StopAsync();
		Task ResetAsync();
	}
}
