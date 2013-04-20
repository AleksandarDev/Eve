using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveWindowsPhone.Modules {
	[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
	public class Module : System.Attribute {
		public string ID { get; private set; }
		public string Image { get; private set; }
		public string Name { get; private set; }
		public string View { get; private set; }


		public Module(string id, string name, string image, string view) {
			this.ID = id;
			this.Name = name ?? "Unknown";
			this.Image = image;
			this.View = view;
		}
	}
}
