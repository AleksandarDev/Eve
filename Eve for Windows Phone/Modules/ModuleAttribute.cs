using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveWindowsPhone.Modules {
	[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
	public class ModuleAttribute : System.Attribute {
		public string ID { get; private set; }
		public string Image { get; private set; }
		public string Name { get; private set; }
		public string View { get; private set; }

		public bool IsInternal { get; private set; }
		public bool IsEnabled { get; private set; }


		public ModuleAttribute(string id, string name, string image, string view, bool isEnabled = false, bool isInternal = true) {
			this.ID = id;
			this.Name = name ?? "Unknown";
			this.Image = image;
			this.View = view;

			this.IsInternal = isInternal;
			this.IsEnabled = isEnabled;
		}
	}
}
