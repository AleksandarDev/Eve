using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eve.API.Process;
using Eve.Diagnostics.Logging;

namespace Eve.API.Vision {
	[ProviderDescription("Display Enhancements Provider", typeof(ProcessProvider))]
	public class DisplayEnhancementsProvider : ProviderBase<DisplayEnhancementsProvider> {
		protected override void Initialize() { }
		protected override void Uninitialize() { }

		#region Display zoom

		// TODO Registry values and then reset process

		public async Task ActivateZoomAsync() {
			var startInfo = new System.Diagnostics.ProcessStartInfo("magnify.exe");
			await ProviderManager.ProcessProvider.StartProcessAsync(startInfo);
		}

		public async Task DeactivateZoomAsync() {
			var processes = ProviderManager.ProcessProvider.GetProcesses();
			var matching = processes.Where(p => p.ProcessName.ToLower() == "magnify");
			foreach (var process in matching) {
				await ProviderManager.ProcessProvider.StopProcessAsync(process);
			}
		}

		/// <summary>
		/// Sets current screen magnification to given value
		/// </summary>
		/// <param name="value">Value of zoom (min 100, max 1600)</param>
		/// <param name="activateZoom">
		/// Activates zoom feature if deactivated.
		/// If this is set to false and screen magnifier is running, 
		/// set settings will be overwritten when process is stopped
		/// </param>
		/// <returns>Returns asynchronous void Task</returns>
		public async Task SetZoom(int value, bool activateZoom = true) {
			if (activateZoom)
				await this.DeactivateZoomAsync();

			// Get registry key and set given value
			var registryKeys = this.GetZoomRegistryKey();
			if (registryKeys == null)
				return;
			registryKeys.SetValue("Magnification", value);

			if (activateZoom)
				await this.ActivateZoomAsync();

			this.log.Info("Zoom set to {0}", value);
		}

		public void ZoomInitialSetup() {
			// TODO Implement customization to initial setup
			this.log.Info("Setting Zoom to initial values...");

			// Get registry key
			var registryKeys = this.GetZoomRegistryKey();
			if (registryKeys == null)
				return;

			// Set initial value to 100%
			registryKeys.SetValue("Magnification", "100",
				Microsoft.Win32.RegistryValueKind.DWord);

			// Set magnification step
			registryKeys.SetValue("ZoomIncrement", "25",
				Microsoft.Win32.RegistryValueKind.DWord);

			// Set follow option to all three
			registryKeys.SetValue("FollowCaret", "1",
				Microsoft.Win32.RegistryValueKind.DWord);
			registryKeys.SetValue("FollowFocus", "1",
				Microsoft.Win32.RegistryValueKind.DWord);
			registryKeys.SetValue("FollowMouse", "1",
				Microsoft.Win32.RegistryValueKind.DWord);

			// Set magnification mode to - full screen
			registryKeys.SetValue("MagnificationMode", "2",
				Microsoft.Win32.RegistryValueKind.DWord);

			this.log.Info("Value for Zoom set");
		}

		private Microsoft.Win32.RegistryKey GetZoomRegistryKey() {
			Microsoft.Win32.RegistryKey registryKeys = Microsoft.Win32.Registry.CurrentUser;
			registryKeys = registryKeys.OpenSubKey("Software\\Microsoft\\ScreenMagnifier\\", true);

			if (registryKeys == null)
				this.log.Warn("Couldn't access screen magnifier registry location");

			return registryKeys;
		}

		#endregion

		#region Properties

		#endregion
	}
}
