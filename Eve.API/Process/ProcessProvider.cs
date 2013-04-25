using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Eve.Diagnostics.Logging;
using Processes = System.Diagnostics;

namespace Eve.API.Process {
	public static class ProcessProvider {
		// TODO Check if service started
		private const int ProcessExitWaitDefault = 500;

		private static readonly Log.LogInstance log =
			new Log.LogInstance(typeof(ProcessProvider));
		private static List<Processes.Process> associated;

		/// <summary>
		/// Starts provider and its components
		/// </summary>
		/// <returns>Returns asynchronous void Task</returns>
		public async static Task Start() {
			// Make sure we don't start provider twice
			if (ProcessProvider.IsRunning) {
				ProcessProvider.log.Warn("Provider is already started");
				return;
			}

			ProcessProvider.log.Info("Starting provider...");

			// Initialize variables
			ProcessProvider.associated = new List<Processes.Process>();
			ProcessProvider.ProcessExitWait = ProcessProvider.ProcessExitWaitDefault;

			ProcessProvider.IsRunning = true;
			ProcessProvider.log.Info("Provider started");
		}

		/// <summary>
		/// Stops provider and its components
		/// </summary>
		/// <returns>Returns asynchronous void Task</returns>
		public async static Task Stop() {
			ProcessProvider.log.Info("Stopping provider...");

			// Close all associated processes
			foreach (var process in ProcessProvider.associated) 
				await ProcessProvider.StopProcessAsync(process);
			ProcessProvider.associated.Clear();

			ProcessProvider.IsRunning = false;
			ProcessProvider.log.Info("Provider stopped");
		}

		/// <summary>
		/// Starts new process using given process start information
		/// </summary>
		/// <param name="startInfo">Information used to start new process</param>
		/// <param name="associateWithClient">Whether to make association with current client. This will make process exit upon closing client</param>
		/// <param name="multiProcess">If set to true, multiple same processes can be started</param>
		/// <returns>Returns new Process object that is associated with the process resource, or null if no process resources is started</returns>
		/// <exception cref="System.ArgumentNullException">The startInfo parameter is null</exception>
		public static async Task<Processes.Process> StartProcessAsync(
			Processes.ProcessStartInfo startInfo,
			bool associateWithClient = false,
			bool multiProcess = false) {
			if (!ProcessProvider.CheckIsRunning())
				return null;

			return await Task.Run(() =>
				ProcessProvider.StartProcess(
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
		public static Processes.Process StartProcess(Processes.ProcessStartInfo startInfo, bool associateWithClient = false, bool multiProcess = false) {
			if (!ProcessProvider.CheckIsRunning())
				return null;
			if (startInfo == null)
				throw new ArgumentNullException("startInfo");

			if (!multiProcess) {
				// Check if process is already started
				var processName =
					System.IO.Path.GetFileNameWithoutExtension(startInfo.FileName);
				if (ProcessProvider.GetProcessesNames().Any(p => p.ToLower() == processName)) {
					ProcessProvider.log.Warn("Process is already running [{0}]", processName);
					return null;
				}
			}

			// Try to start process using given information
			try {
				return Processes.Process.Start(startInfo);
			}
			catch (InvalidOperationException ex) {
				ProcessProvider.log.Error<InvalidOperationException>(
					ex, "Unable to start process [{0}]", startInfo.FileName);
				return null;
			}
			catch (System.IO.FileNotFoundException ex) {
				ProcessProvider.log.Error<System.IO.FileNotFoundException>(
					ex, "Couldn't locate process file [{0}]", startInfo.FileName);
				return null;
			} catch (Exception ex) {
				ProcessProvider.log.Error<Exception>(
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
		public async static Task StopProcessAsync(Processes.Process process) {
			if (!ProcessProvider.CheckIsRunning())
				return;

			await Task.Run(() => ProcessProvider.StopProcess(process));
		}

		/// <summary>
		/// Stops given process
		/// Calls CloseMainWindow on given process and waits for 500ms for 
		/// process to exit, if process doesn't exit till then Kill is called
		/// </summary>
		/// <param name="process">Process to kill</param>
		/// <exception cref="System.ArgumentNullException">The process parameter is null</exception>
		public static void StopProcess(Processes.Process process) {
			if (!ProcessProvider.CheckIsRunning())
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
				ProcessProvider.log.Error<Win32Exception>(
					ex, "Process couldn't be terminated or is already terminating [{0}]",
					process.ProcessName);
				return;
			}
			catch (NotSupportedException ex) {
				ProcessProvider.log.Error<NotSupportedException>(
					ex,
					"You are attempting to call Kill for process that is running on remote computer [{0}]",
					process.ProcessName);
			}
			catch (InvalidOperationException ex) {
				ProcessProvider.log.Error<InvalidOperationException>(
					ex,
					"The process ha already exited or there is no process associated with given Process object [{0}]",
					process.ProcessName);
			}
		}

		/// <summary>
		/// Finds all running processes
		/// </summary>
		/// <returns>Returns a list of running processes names</returns>
		public static IEnumerable<string> GetProcessesNames() {
			if (!ProcessProvider.CheckIsRunning())
				return null;
			return ProcessProvider.GetProcesses().Select(p => p.ProcessName);
		} 

		/// <summary>
		/// Finds all running processes
		/// </summary>
		/// <returns>Returns a list of running processes</returns>
		public static IEnumerable<Processes.Process> GetProcesses() {
			if (!ProcessProvider.CheckIsRunning())
				return null;
			return Processes.Process.GetProcesses().AsEnumerable();
		}

		/// <summary>
		/// Check whether provider is running - writes warning if it's not running
		/// </summary>
		/// <returns>Returns Boolean value indicating whether provider is started</returns>
		private static bool CheckIsRunning() {
			if (!ProcessProvider.IsRunning)
				ProcessProvider.log.Warn("Start provider before using it!");

			return ProcessProvider.IsRunning;
		}

		#region Properties

		/// <summary>
		/// Gets indication whether provider is running
		/// </summary>
		public static bool IsRunning { get; private set; }

		/// <summary>
		/// Gets or sets wait time for process to close before forcing it to kill
		/// </summary>
		public static int ProcessExitWait { get; set; }

		#endregion
	}
}
