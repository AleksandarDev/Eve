using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eve.Diagnostics.Logging;

namespace Eve.API.Vision {
	public static class DisplayEnhancementsProvider {
		private static readonly Log.LogInstance log =
			new Log.LogInstance(typeof(DisplayEnhancementsProvider));

		/// <summary>
		/// Starts provider and initializes components
		/// </summary>
		/// <returns>Returns asynchronous void Task</returns>
		public static async Task Start() {
			// Make sure we don't start provider twice
			if (DisplayEnhancementsProvider.IsRunning) {
				DisplayEnhancementsProvider.log.Warn("Provider is already started");
				return;
			}

			DisplayEnhancementsProvider.log.Info("Starting provider...");

			DisplayEnhancementsProvider.IsRunning = true;
			DisplayEnhancementsProvider.log.Info("Provider started");
		}

		/// <summary>
		/// Stops provider and its components
		/// </summary>
		/// <returns>Returns asynchronous void Task</returns>
		public static async Task Stop() {
			DisplayEnhancementsProvider.log.Info("Stopping provider...");

			DisplayEnhancementsProvider.IsRunning = false;
			DisplayEnhancementsProvider.log.Info("Provider stopped");
		}

		#region Display zoom

		// TODO Registry values and then reset process

		public static void ActivateZoom() {}

		public static void DeactivateZoom() {}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value">Value of zoom</param>
		/// <param name="activateZoom">Activates zoom feature if deactivated</param>
		public static void SetZoom(double value, bool activateZoom = true) {}

		#endregion

		/// <summary>
		/// Check whether provider is running - writes warning if it's not running
		/// </summary>
		/// <returns>Returns Boolean value indicating whether provider is started</returns>
		private static bool CheckIsRunning() {
			if (!DisplayEnhancementsProvider.IsRunning)
				DisplayEnhancementsProvider.log.Warn("Start provider before using it!");

			return DisplayEnhancementsProvider.IsRunning;
		}

		#region Properties

		/// <summary>
		/// Gets indication whether provider is running
		/// </summary>
		public static bool IsRunning { get; private set; }

		#endregion
	}
}
