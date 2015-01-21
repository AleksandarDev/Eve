using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Eve.API.Chrome;
using Eve.API.Touch;
using Eve.API.Process;
using Eve.API.Scripting;
using Eve.API.Speech;
using Eve.API.Text;
using Eve.API.Vision;
using Eve.Diagnostics.Logging;

namespace Eve.API {
	/// <summary>
	/// Gives access and manages all available providers
	/// </summary>
	public static class ProviderManager {
		// BUG when provider doesn't have description, and exception occurs
		private static readonly Log.LogInstance log =
			new Log.LogInstance(typeof(ProviderManager));

		private static IEnumerable<IProvider> providers;

		public delegate void ProviderManagerEventHandler();
		public static event ProviderManagerEventHandler OnInitialized;


		static ProviderManager() {
			ProviderManager.providers = ProviderManager.GetSortedProviders();

			ProviderManager.TouchProvider = ProviderManager.providers.OfType<TouchProvider>().First();
			ProviderManager.ProcessProvider = ProviderManager.providers.OfType<ProcessProvider>().First();
			ProviderManager.ScriptingProvider = ProviderManager.providers.OfType<ScriptingProvider>().First();
			ProviderManager.SpeechProvider = ProviderManager.providers.OfType<SpeechProvider>().First();
			ProviderManager.TextProvider = ProviderManager.providers.OfType<TextProvider>().First();
			ProviderManager.CaptchaDecoderProvider = ProviderManager.providers.OfType<CaptchaDecoderProvider>().First();
			ProviderManager.DisplayEnhancementsProvider = ProviderManager.providers.OfType<DisplayEnhancementsProvider>().First();
			ProviderManager.VideoProvider = ProviderManager.providers.OfType<VideoProvider>().First();
			ProviderManager.FaceDetectionProvider = ProviderManager.providers.OfType<FaceDetectionProvider>().First();
			ProviderManager.FaceControllerProvider = ProviderManager.providers.OfType<FaceControllerProvider>().First();
			ProviderManager.ScreenViewerProvider = ProviderManager.providers.OfType<ScreenViewerProvider>().First();
			ProviderManager.ChromeProvider = ProviderManager.providers.OfType<ChromeProvider>().First();

			ProviderManager.IsInitialized = true;
			if (ProviderManager.OnInitialized != null)
				ProviderManager.OnInitialized();
		}

		/// <summary>
		/// Starts all available providers
		/// </summary>
		/// <returns>Returns asynchronous void Task</returns>
		public static async Task StartAsync() {
			ProviderManager.log.Info("Starting providers...");

			foreach (var provider in ProviderManager.providers) {
				await provider.StartAsync();
			}

			ProviderManager.IsStarted = true;
			ProviderManager.log.Info("All Providers started");
		}

		/// <summary>
		/// Stops all available providers
		/// </summary>
		/// <returns>Returns asynchronous void Task</returns>
		public static async Task StopAsync() {
			ProviderManager.log.Info("Stopping providers...");

			foreach (var provider in ProviderManager.providers.Reverse()) {
				await provider.StopAsync();
			}

			ProviderManager.IsStarted = false;
			ProviderManager.log.Info("All Provider stopped");
		}

		private static IEnumerable<IProvider> GetSortedProviders() {
			// TODO Test sort (not tested)

			// Retrieve all providers (distinct)
			var providerTypes = System.Reflection.Assembly.GetExecutingAssembly()
									  .GetTypes()
									  .Where(
										  t =>
										  typeof(IProvider).IsAssignableFrom(t) && 
										  !t.IsInterface && !t.IsAbstract && !t.IsSealed)
									  .Distinct().ToList();

			var providerInstances = new List<IProvider>();

			// Sort list according to dependencies
			for (int index = 0; index < providerTypes.Count; index++) {
				// Retrieve description
				var description = ProviderManager.RetrieveDescription(
					providerTypes.ElementAt(index));

				// Skip if doesn't contain any dependencies
				if (description == null || 
					description.Dependencies == null ||
					description.Dependencies.Length == 0) continue;

				// Sort dependencies
				foreach (var dependency in description.Dependencies) {
					// Skip self dependent
					if (dependency == providerTypes[index]) continue;

					// Find type in list and move it one place before current provider
					int dependencyIndex = -1;
					for (int typeIndex = index + 1; typeIndex < providerTypes.Count; typeIndex++)
						if (providerTypes[typeIndex] == dependency) {
							dependencyIndex = typeIndex;
							break;
						}

					// Check if we found dependency type on the list
					if (dependencyIndex <= 0) {
						ProviderManager.log.Warn("Couldn't find dependency type on list [{0}]",
												 dependency.Name);
						continue;
					}

					// Check if type is already higher priority
					if (dependencyIndex < index) continue;

					// Move dependency one place before current provider type
					providerTypes.Insert(index, providerTypes.ElementAt(dependencyIndex));
					providerTypes.RemoveAt(dependencyIndex + 1);
					index--;
				}
			}

			// Build list of providers instances
			foreach (var type in providerTypes) {
				var instance = Activator.CreateInstance(type);
				var providerInstance = instance as IProvider;
				if (providerInstance != null)
					providerInstances.Add(providerInstance);
				else
					ProviderManager.log.Warn("Couldn't create instance of [{0}]", type.Name);
			}

			return providerInstances;
		} 

		/// <summary>
		/// Retrieves provider description from given provider type
		/// </summary>
		/// <param name="providerType">Type of provider to retrieve description for</param>
		/// <returns>Returns description of given provider type</returns>
		public static ProviderDescription RetrieveDescription(Type providerType) {
			// Retrieve attributes
			var attributes = Attribute.GetCustomAttributes(providerType);

			// Check if there is more than one provider description
			if (attributes.OfType<ProviderDescription>().Count() > 1)
				throw new InvalidOperationException(
					"Provider can only have one description");

			// Check if there is any provider description available
			if (!attributes.OfType<ProviderDescription>().Any())
				throw new NotSupportedException("Can't find description for provider");

			return attributes.First() as ProviderDescription;
		}

		#region Providers

		public static bool IsInitialized { get; private set; }
		public static bool IsStarted { get; private set; }
		public static IEnumerable<IProvider> Providers { get { return ProviderManager.providers; } } 

		public static TouchProvider TouchProvider { get; private set; }
		public static ProcessProvider ProcessProvider { get; private set; }
		public static ScriptingProvider ScriptingProvider { get; private set; }
		public static SpeechProvider SpeechProvider { get; private set; }
		public static TextProvider TextProvider { get; private set; }
		public static CaptchaDecoderProvider CaptchaDecoderProvider { get; private set; }
		public static DisplayEnhancementsProvider DisplayEnhancementsProvider { get; private set; }
		public static VideoProvider VideoProvider { get; private set; }
		public static FaceDetectionProvider FaceDetectionProvider { get; private set; }
		public static FaceControllerProvider FaceControllerProvider { get; private set; }
		public static ScreenViewerProvider ScreenViewerProvider { get; private set; }
		public static ChromeProvider ChromeProvider { get; private set; }

		#endregion
	}
}