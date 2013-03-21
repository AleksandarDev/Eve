using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveWindowsPhone.Modules {
	[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
	public class Module : System.Attribute {
		public string Image { get; private set; }
		public string Name { get; private set; }
		public Type View { get; private set; }


		public Module(string name, string image, Type view) {
			this.Name = name ?? "Unknown";
			this.Image = image;
			this.View = view;
		}
	}
}
