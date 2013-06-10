using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using EveWindowsPhone.Services;
using EveWindowsPhone.ViewModels;

namespace EveWindowsPhone.Modules {
	public class ModulesLocator : NotificationObject {
		public ModulesLocator() {
			this.AvailableModules = new ObservableCollection<ModuleModel>();
			this.OwnedModules = new ObservableCollection<ModuleModel>();
			this.LoadModules();
		}


		private void LoadModules() {
			// Find all classes that are qualified to be parent of an module attribute types that are static 
			var availableModuleTypes = System.Reflection.Assembly.GetExecutingAssembly()
			                                 .GetTypes()
			                                 .Where(ModulesLocator.CanBeModuleParent);

			// Go through found classes and look for Module attribute
			foreach (var moduleType in availableModuleTypes) {
				var attributes = Attribute.GetCustomAttributes(moduleType);
				if (attributes.Any()) {
					foreach (var attribute in attributes.OfType<ModuleAttribute>()) {
#if !DEBUG
						if (!attribute.IsInternal) 
#endif
							if (attribute.IsEnabled)
								this.OwnedModules.Add(new ModuleModel(attribute));
							else this.AvailableModules.Add(new ModuleModel(attribute));
					}
				}
			}
		}

		public static bool CanBeModuleParent(Type type) {
			return typeof (NotificationObject).IsAssignableFrom(type) &&
			       !type.IsAbstract && !type.IsInterface;
		}

		#region Properties

		public ObservableCollection<ModuleModel> AvailableModules { get; private set; }
		public ObservableCollection<ModuleModel> OwnedModules { get; private set; } 

		#endregion
	}
}
