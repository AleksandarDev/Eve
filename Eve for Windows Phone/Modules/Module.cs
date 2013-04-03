using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveWindowsPhone.ViewModels;

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

	public class ModuleModel : NotificationObject {
		private bool isEditing;
		private bool isFavorite;


		public ModuleModel(Module module) {
			this.Module = module;
		}


		#region Properties

		public Module Module { get; private set; }

		public bool IsFavorite {
			get { return this.isFavorite; }
			set {
				this.isFavorite = value;
				RaisePropertyChanged("IsFavorite");
			}
		}

		public bool IsEditing {
			get { return this.isEditing; }
			set {
				this.isEditing = value;
				RaisePropertyChanged("IsEditing");
			}
		}

		#endregion
	}
}
