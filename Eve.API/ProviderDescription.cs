using System;
using System.Linq;

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

			if (dependencies.Any(d => !typeof(IProvider).IsAssignableFrom(d)))
				throw new InvalidCastException("Some dependencies aren't of required type");
		}
	}

	//[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
	//public class ProviderDependency : Attribute {
	//	public string TypeFullName { get; private set; }

	//	public ProviderDependency(Type type) : this(type.FullName) { }
	//	public ProviderDependency(string typeFullName) {
	//		if (String.IsNullOrEmpty(typeFullName))
	//			throw new InvalidOperationException("Type full name can't be empty or null");

	//		if (!typeof(IProvider).IsAssignableFrom(Type.GetType(typeFullName)))
	//			throw new InvalidCastException("Dependency isn't of required type!");

	//		this.TypeFullName = typeFullName;
	//	}
	//}
}