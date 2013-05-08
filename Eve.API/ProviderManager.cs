using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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
		private static readonly Log.LogInstance log =
			new Log.LogInstance(typeof(ProviderManager));

		/// <summary>
		/// Creates all provider objects
		/// </summary>
		static ProviderManager() {
			//var providers = ProviderManager.GetSortedProviders();

			ProviderManager.TouchProvider = new TouchProvider();
			ProviderManager.ProcessProvider = new ProcessProvider();
			ProviderManager.ScriptingProvider = new ScriptingProvider();
			ProviderManager.SpeechProvider = new SpeechProvider();
			ProviderManager.TextProvider = new TextProvider();
			ProviderManager.CaptchaDecoderProvider = new CaptchaDecoderProvider();
			ProviderManager.DisplayEnhancementsProvider = new DisplayEnhancementsProvider();
			ProviderManager.VideoProvider = new VideoProvider();
			ProviderManager.FaceDetectionProvider = new FaceDetectionProvider();
			ProviderManager.ScreenViewerProvider = new ScreenViewerProvider();			
		}

		/// <summary>
		/// Starts all available providers
		/// </summary>
		/// <returns>Returns asynchronous void Task</returns>
		public static async Task StartAsync() {
			ProviderManager.log.Info("Starting providers...");

			await ProviderManager.TouchProvider.StartAsync();
			await ProviderManager.ProcessProvider.StartAsync();
			await ProviderManager.ScriptingProvider.StartAsync();
			await ProviderManager.SpeechProvider.StartAsync();
			await ProviderManager.TextProvider.StartAsync();
			await ProviderManager.CaptchaDecoderProvider.StartAsync();
			await ProviderManager.DisplayEnhancementsProvider.StartAsync();
			await ProviderManager.VideoProvider.StartAsync();
			await ProviderManager.FaceDetectionProvider.StartAsync();
			await ProviderManager.ScreenViewerProvider.StartAsync();

			ProviderManager.IsStarted = true;
			ProviderManager.log.Info("All Provider started");
		}

		/// <summary>
		/// Stops all available providers
		/// </summary>
		/// <returns>Returns asynchronous void Task</returns>
		public static async Task StopAsync() {
			ProviderManager.log.Info("Stopping providers...");

			await ProviderManager.TouchProvider.StopAsync();
			await ProviderManager.ProcessProvider.StopAsync();
			await ProviderManager.ScriptingProvider.StopAsync();
			await ProviderManager.SpeechProvider.StopAsync();
			await ProviderManager.TextProvider.StopAsync();
			await ProviderManager.CaptchaDecoderProvider.StopAsync();
			await ProviderManager.DisplayEnhancementsProvider.StopAsync();
			await ProviderManager.VideoProvider.StopAsync();
			await ProviderManager.FaceDetectionProvider.StopAsync();
			await ProviderManager.ScreenViewerProvider.StopAsync();

			ProviderManager.IsStarted = false;
			ProviderManager.log.Info("All Provider stopped");
		}

		public static IEnumerable<IProvider> GetSortedProviders() {
			// Retrieve all providers
			var providers = System.Reflection.Assembly.GetExecutingAssembly()
				.GetTypes().Where(t => t.IsAssignableFrom(typeof(IProvider)) && t.IsInterface);

			foreach (var provider in providers) {
				// TODO Implement sort algorithm
			}
			// Build lis t of providers
			//return providers;
			throw new NotImplementedException();
		} 

		/// <summary>
		/// Retrieves provider description from given provider type
		/// </summary>
		/// <param name="providerType">Type of provider to retrieve description for</param>
		/// <returns>Returns description of given provider type</returns>
		private static ProviderDescription RetrieveDescription(Type providerType) {
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

		public static bool IsStarted { get; private set; }

		public static TouchProvider TouchProvider { get; private set; }
		public static ProcessProvider ProcessProvider { get; private set; }
		public static ScriptingProvider ScriptingProvider { get; private set; }
		public static SpeechProvider SpeechProvider { get; private set; }
		public static TextProvider TextProvider { get; private set; }
		public static CaptchaDecoderProvider CaptchaDecoderProvider { get; private set; }
		public static DisplayEnhancementsProvider DisplayEnhancementsProvider { get; private set; }
		public static VideoProvider VideoProvider { get; private set; }
		public static FaceDetectionProvider FaceDetectionProvider { get; private set; }
		public static ScreenViewerProvider ScreenViewerProvider { get; private set; }

		#endregion
	}
}