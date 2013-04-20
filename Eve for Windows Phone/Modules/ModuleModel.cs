using EveWindowsPhone.ViewModels;

namespace EveWindowsPhone.Modules {
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

		public override string ToString() {
			if (this.Module == null)
				return "Unknown";
			return this.Module.Name;
		}
	}
}