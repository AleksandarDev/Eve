using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accord.Statistics.Models.Fields.Features;
using Eve.Diagnostics.Logging;
using Processes = System.Diagnostics;

namespace Eve.API.Process {
	[ProviderDescription("Process Provider")]
	public class ProcessProvider : ProviderBase<ProcessProvider> {
		// TODO Check if service started
		private const int ProcessExitWaitDefault = 500;

		private List<Processes.Process> associated;


		protected override void Initialize() {
			// Initialize variables
			this.associated = new List<Processes.Process>();
			this.ProcessExitWait = ProcessProvider.ProcessExitWaitDefault;
		}

		protected override void Uninitialize() {
			// Close all associated processes
			foreach (var process in this.associated)
				this.StopProcess(process);
			this.associated.Clear();
		}

		/// <summary>
		/// Starts new process using given process start information
		/// </summary>
		/// <param name="startInfo">Information used to start new process</param>
		/// <param name="associateWithClient">Whether to make association with current client. This will make process exit upon closing client</param>
		/// <param name="multiProcess">If set to true, multiple same processes can be started</param>
		/// <returns>Returns new Process object that is associated with the process resource, or null if no process resources is started</returns>
		/// <exception cref="System.ArgumentNullException">The startInfo parameter is null</exception>
		public async Task<Processes.Process> StartProcessAsync(
			Processes.ProcessStartInfo startInfo,
			bool associateWithClient = false,
			bool multiProcess = false) {
				if (!this.CheckIsRunning())
				return null;

			return await Task.Run(() =>
				this.StartProcess(
					startInfo, associateWithClient, multiProcess));
		}

		/// <summary>
		/// Starts new process using given process start information
		/// </summary>
		/// <param name="startInfo">Information used to start new process</param>
		/// <param name="associateWithClient">Whether to make association with current client. This will make process exit upon closing client</param>
		/// <param name="multiProcess">If set to true, multiple same processes can be started</param>
		/// <returns>Returns new Process object that is associated with the process resource, or null if no process resources is started</returns>
		/// <exception cref="System.ArgumentNullException">The startInfo parameter is null</exception>
		public Processes.Process StartProcess(Processes.ProcessStartInfo startInfo, bool associateWithClient = false, bool multiProcess = false) {
			if (!this.CheckIsRunning())
				return null;
			if (startInfo == null)
				throw new ArgumentNullException("startInfo");

			if (!multiProcess) {
				// Check if process is already started
				var processName =
					System.IO.Path.GetFileNameWithoutExtension(startInfo.FileName);
				if (this.GetProcessesNames().Any(p => p.ToLower() == processName)) {
					this.log.Warn("Process is already running [{0}]", processName);
					return null;
				}
			}

			// Try to start process using given information
			try {
				var process = Processes.Process.Start(startInfo);

				if (associateWithClient)
					this.associated.Add(process);

				return process;
			}
			catch (InvalidOperationException ex) {
				this.log.Error<InvalidOperationException>(
					ex, "Unable to start process [{0}]", startInfo.FileName);
				return null;
			}
			catch (System.IO.FileNotFoundException ex) {
				this.log.Error<System.IO.FileNotFoundException>(
					ex, "Couldn't locate process file [{0}]", startInfo.FileName);
				return null;
			} catch (Exception ex) {
				this.log.Error<Exception>(
					ex, "Unexpected error occurred while starting process [{0}]",
					startInfo.FileName);
				return null;
			}
		}

		/// <summary>
		/// Stops given process
		/// Calls CloseMainWindow on given process and waits for 500ms for 
		/// process to exit, if process doesn't exit till then Kill is called
		/// </summary>
		/// <param name="process">Process to kill</param>
		/// <exception cref="System.ArgumentNullException">The process parameter is null</exception>
		public async Task StopProcessAsync(Processes.Process process) {
			if (!this.CheckIsRunning())
				return;

			await Task.Run(() => this.StopProcess(process));
		}

		/// <summary>
		/// Stops given process
		/// Calls CloseMainWindow on given process and waits for 500ms for 
		/// process to exit, if process doesn't exit till then Kill is called
		/// </summary>
		/// <param name="process">Process to kill</param>
		/// <exception cref="System.ArgumentNullException">The process parameter is null</exception>
		public virtual void StopProcess(Processes.Process process) {
			if (!this.CheckIsRunning())
				return;
			if (process == null)
				throw new ArgumentNullException("process");

			try {
				// Ask for exit and wait for 500ms
				process.CloseMainWindow();
				process.WaitForExit(500);

				// If process is still running, force kill
				if (!process.HasExited) {
					process.Kill();
					process.WaitForExit();
				}
			}
			catch (Win32Exception ex) {
				this.log.Error<Win32Exception>(ex, 
					"Process couldn't be terminated or is already terminating [{0}]",
					process.ProcessName);
				return;
			}
			catch (NotSupportedException ex) {
				this.log.Error<NotSupportedException>(ex,
					"You are attempting to call Kill for process that is running on remote computer [{0}]",
					process.ProcessName);
			}
			catch (InvalidOperationException ex) {
				this.log.Error<InvalidOperationException>(ex,
					"The process has already exited or there is no process associated with given Process object [{0}]",
					process.ProcessName);
			}
			catch (Exception ex) {
				this.log.Error<Exception>(ex,
					"Unknown exception occured while trying to exit process [{0}]",
					process.ProcessName);
			}
		}

		/// <summary>
		/// Finds all running processes
		/// </summary>
		/// <returns>Returns a list of running processes names</returns>
		public IEnumerable<string> GetProcessesNames() {
			if (!this.CheckIsRunning())
				return null;
			return this.GetProcesses().Select(p => p.ProcessName);
		} 

		/// <summary>
		/// Finds all running processes
		/// </summary>
		/// <returns>Returns a list of running processes</returns>
		public IEnumerable<Processes.Process> GetProcesses() {
			if (!this.CheckIsRunning())
				return null;
			return Processes.Process.GetProcesses().AsEnumerable();
		}


		#region Properties

		/// <summary>
		/// Gets or sets wait time for process to close before forcing it to kill
		/// </summary>
		public int ProcessExitWait { get; set; }

		#endregion
	}
}
