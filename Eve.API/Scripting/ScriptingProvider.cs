using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Eve.Diagnostics.Logging;
using Microsoft.CSharp;

namespace Eve.API.Scripting {
	public static class ScriptingProvider {
		public const string DefaultScriptEntryPointName = "Initiate";

		private static readonly Log.LogInstance log =
			new Log.LogInstance(typeof(ScriptingProvider));

		private static CSharpCodeProvider codeProvider;
		private static CompilerParameters codeCompilerParameters;


		public static async Task Start() {
			if (ScriptingProvider.IsRunning) {
				ScriptingProvider.log.Warn("Provider is already started");
				return;
			}

			ScriptingProvider.log.Info("Starting provider...");

			await Task.Run(() => ScriptingProvider.Initialize());

			ScriptingProvider.IsRunning = true;
			ScriptingProvider.log.Info("Provider started");
		}

		private static void Initialize() {
			// Create code provider and set parameters
			ScriptingProvider.codeProvider = new CSharpCodeProvider();
			ScriptingProvider.codeCompilerParameters = new CompilerParameters() {
				GenerateExecutable = false,
				GenerateInMemory = true
			};

			// Assign default referenced assemblies
			ScriptingProvider.codeCompilerParameters.ReferencedAssemblies.Add("System.dll");
			ScriptingProvider.codeCompilerParameters.ReferencedAssemblies.Add("System.Core.dll");
			ScriptingProvider.codeCompilerParameters.ReferencedAssemblies.Add("System.Data.dll");
			ScriptingProvider.codeCompilerParameters.ReferencedAssemblies.Add("System.Data.DataSetExtensions.dll");
			ScriptingProvider.codeCompilerParameters.ReferencedAssemblies.Add("System.Xml.dll");
			ScriptingProvider.codeCompilerParameters.ReferencedAssemblies.Add("System.Xml.Linq.dll");
			ScriptingProvider.codeCompilerParameters.ReferencedAssemblies.Add("Eve.Core.dll");
			ScriptingProvider.codeCompilerParameters.ReferencedAssemblies.Add("Eve.API.dll");

			ScriptingProvider.IsRunning = true;
		}

		public static async Task Stop() {
			ScriptingProvider.log.Info("Stopping provider...");

			await Task.Run(() => {
				if (ScriptingProvider.codeProvider != null) {
					ScriptingProvider.codeProvider.Dispose();
					ScriptingProvider.codeProvider = null;
				}
				ScriptingProvider.codeCompilerParameters = null;
			});

			ScriptingProvider.IsRunning = false;
			ScriptingProvider.log.Info("Provider stopped");
		}

		public static async Task<Tuple<bool, CompilerResults>> RunScriptAsync(params string[] scriptCode) {
			if (!ScriptingProvider.CheckIsRunning())
				return null;

			// Compile given code
			var compiled = await ScriptingProvider.CompileScriptAsync(scriptCode);

			// Run script if no errors were found during code compilation
			if (!compiled.Errors.HasErrors) {
				bool executionResult = await ScriptingProvider.RunScriptAsync(compiled.CompiledAssembly);
				return new Tuple<bool, CompilerResults>(executionResult, compiled);
			}

			return new Tuple<bool, CompilerResults>(false, compiled);
		}

		public static async Task<bool> RunScriptAsync(Assembly scriptAssembly) {
			if (!ScriptingProvider.CheckIsRunning())
				return false;

			var scripts = scriptAssembly.GetTypes().Where(type => typeof (IScript).IsAssignableFrom(type)).ToList();
			if (scripts.Count != 1) {
				System.Diagnostics.Debug.WriteLine("There any no/too many scripts defined in assembly!",
				                                   typeof (ScriptingProvider).Name);
				// TODO Throw exception
				return false;
			}

			// Create instance of script class
			var scriptInstance = scriptAssembly.CreateInstance(scripts.FirstOrDefault().FullName);
			if (scriptInstance == null) {
				System.Diagnostics.Debug.WriteLine(
					String.Format("Given assembly doesn't contains needed type \"{0}\"!", typeof (IScript).FullName),
					typeof (ScriptingProvider).Name);
				// TODO Throw exception
				return false;
			}

			// Check if entry method exists
			var availableMethods = scriptInstance.GetType().GetMethods();
			if (availableMethods.All(method => method.Name != DefaultScriptEntryPointName)) {
				System.Diagnostics.Debug.WriteLine(
					String.Format("Script doesn't contain needed entry point \"{0}\"", DefaultScriptEntryPointName),
					typeof (ScriptingProvider).Name);
				// TODO Throw exception
				return false;
			}

			// Run method asynchronously and wait for result
			bool executionResult = await Task.Run(() => {
				try {
					// Invoke entry script method
					scriptInstance.GetType().GetMethod(DefaultScriptEntryPointName).Invoke(scriptInstance, new object[0]);
				}
				catch (Exception ex) {
					System.Diagnostics.Debug.WriteLine("Exception occurred while executing script: " + ex.Message,
					                                   typeof (ScriptingProvider).Name);
					System.Diagnostics.Debug.WriteLine(ex, typeof (ScriptingProvider).Name);
					// TODO Throw exception
					return false;
				}
				return true;
			});

			// Check if execution went successfully
			if (!executionResult) {
				System.Diagnostics.Debug.WriteLine("An error occurred while executing script!", typeof (ScriptingProvider).Name);
				// TODO Throw exception
			}

			return executionResult;
		}

		public static async Task<CompilerResults> CompileScriptAsync(params string[] scriptCode) {
			if (!ScriptingProvider.CheckIsRunning())
				return null;

			// Run code compilation asynchronously and wait for result
			var compileResult = await Task.Run(() => {
				return ScriptingProvider.codeProvider.CompileAssemblyFromSource(
					ScriptingProvider.codeCompilerParameters, scriptCode);
			});

			// Check if there were any errors while compiling and write 
			// them to output window for debug purposes 
			if (compileResult.Errors.HasErrors) {
				System.Diagnostics.Debug.WriteLine(String.Format("Error({0}) occurred while compiling script:\n",
																 compileResult.Errors.Count), typeof(ScriptingProvider).Name);
				foreach (var error in compileResult.Errors) {
					System.Diagnostics.Debug.WriteLine("\t" + error.ToString(), typeof(ScriptingProvider).Name);
				}
			}

			return compileResult;
		}

		/// <summary>
		/// Check whether provider is running - writes warning if it's not running
		/// </summary>
		/// <returns>Returns Boolean value indicating whether provider is started</returns>
		private static bool CheckIsRunning() {
			if (!ScriptingProvider.IsRunning)
				ScriptingProvider.log.Warn("Start provider before using it!");

			return ScriptingProvider.IsRunning;
		}


		#region Properties

		public static bool IsRunning { get; private set; }

		#endregion
	}
}
