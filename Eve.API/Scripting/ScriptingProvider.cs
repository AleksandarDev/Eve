using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;

namespace Eve.API.Scripting {
	public static class ScriptingProvider {
		public const string DefaultScriptEntryPointName = "Initiate";

		//
		// Service 
		//
		private static bool isServiceInitialized = false;

		//
		// Code compiler
		//
		private static CSharpCodeProvider codeProvider;
		private static CompilerParameters codeCompilerParameters;

		public static async Task Start() {
			await Task.Run(() => {
				// Create code provider and set parameters
				ScriptingProvider.codeProvider = new CSharpCodeProvider();
				ScriptingProvider.codeCompilerParameters = new CompilerParameters() {
					GenerateExecutable = false,
					GenerateInMemory = true
				};

				// Asign default referenced assemblies
				ScriptingProvider.codeCompilerParameters.ReferencedAssemblies.Add("System.dll");
				ScriptingProvider.codeCompilerParameters.ReferencedAssemblies.Add("System.Core.dll");
				ScriptingProvider.codeCompilerParameters.ReferencedAssemblies.Add("System.Data.dll");
				ScriptingProvider.codeCompilerParameters.ReferencedAssemblies.Add("System.Data.DataSetExtensions.dll");
				ScriptingProvider.codeCompilerParameters.ReferencedAssemblies.Add("System.Xml.dll");
				ScriptingProvider.codeCompilerParameters.ReferencedAssemblies.Add("System.Xml.Linq.dll");
				ScriptingProvider.codeCompilerParameters.ReferencedAssemblies.Add("Eve.Core.dll");
				ScriptingProvider.codeCompilerParameters.ReferencedAssemblies.Add("Eve.API.dll");

				ScriptingProvider.isServiceInitialized = true;
			});
		}

		public static async Task<Tuple<bool, CompilerResults>> RunScriptAsync(params string[] scriptCode) {
			// Check if service is initialized
			if (!ScriptingProvider.isServiceInitialized)
				await ScriptingProvider.Start();

			// Compile given code
			var compiled = await ScriptingProvider.CompileScript(scriptCode);

			// Run script if no errors were found during code compilation
			if (!compiled.Errors.HasErrors) {
				bool executionResult = await ScriptingProvider.RunScriptAsync(compiled.CompiledAssembly);
				return new Tuple<bool, CompilerResults>(executionResult, compiled);
			}

			return new Tuple<bool, CompilerResults>(false, compiled);
		}

		public static async Task<bool> RunScriptAsync(Assembly scriptAssembly) {
			// Check if service is initialized
			if (!ScriptingProvider.isServiceInitialized)
				await ScriptingProvider.Start();

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
					String.Format("Given assembly doesn't containe needed type \"{0}\"!", typeof (IScript).FullName),
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

			// Run method async and wait for result
			bool executionResult = await Task.Run(() => {
				try {
					// Invoke entry script method
					scriptInstance.GetType().GetMethod(DefaultScriptEntryPointName).Invoke(scriptInstance, new object[0]);
				}
				catch (Exception ex) {
					System.Diagnostics.Debug.WriteLine("Exception occured while executing script: " + ex.Message,
					                                   typeof (ScriptingProvider).Name);
					System.Diagnostics.Debug.WriteLine(ex, typeof (ScriptingProvider).Name);
					// TODO Throw exception
					return false;
				}
				return true;
			});

			// Check if execution went successfully
			if (!executionResult) {
				System.Diagnostics.Debug.WriteLine("An error occured while executing script!", typeof (ScriptingProvider).Name);
				// TODO Throw exception
			}

			return executionResult;
		}

		public static async Task<CompilerResults> CompileScript(params string[] scriptCode) {
			// Check if service is initialized
			if (!ScriptingProvider.isServiceInitialized)
				ScriptingProvider.Start();

			// Run code compilation async and wait for result
			var compileResult = await Task.Run(() => {
				return ScriptingProvider.codeProvider.CompileAssemblyFromSource(
					ScriptingProvider.codeCompilerParameters, scriptCode);
			});

			// Check if there were any errors while compiling and write 
			// them to output window for debug purposes 
			if (compileResult.Errors.HasErrors) {
				System.Diagnostics.Debug.WriteLine(String.Format("Error({0}) occured while compiling script:\n",
																 compileResult.Errors.Count), typeof(ScriptingProvider).Name);
				foreach (var error in compileResult.Errors) {
					System.Diagnostics.Debug.WriteLine("\t" + error.ToString(), typeof(ScriptingProvider).Name);
				}
			}

			return compileResult;
		}

		public static async Task Stop() {
			await Task.Run(() => {
				if (ScriptingProvider.codeProvider != null) {
					ScriptingProvider.codeProvider.Dispose();
					ScriptingProvider.codeProvider = null;
				}
				ScriptingProvider.codeCompilerParameters = null;
				ScriptingProvider.isServiceInitialized = false;
			});
		}
	}
}
