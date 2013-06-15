using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EveControl.Adapters;
using Microsoft.Practices.Prism.ViewModel;

namespace EveControl.Windows.Log {
	public class LogViewModel : NotificationObject, IDisposable {
		private readonly IServerServiceFacade serverServiceFacade;
		private List<Eve.Diagnostics.Logging.Log.LogMessage> messages;
		private List<Eve.Diagnostics.Logging.Log.LogMessage> cachedMessages;
		private List<Type> selectedTypes; 
		private string typeSelected;
		private bool isAutoScroll;
		private bool showDebugMessages;
		private bool showErrorMessages;
		private bool showWarnMessages;
		private bool showInfoMessages;
		private bool showWriteMessages;

		public event EventHandler OnSelectedTypeAdded;
		public event EventHandler OnMessageAdded;

		public LogViewModel(IServerServiceFacade serverServiceFacade) {
			if (serverServiceFacade == null)
				throw new ArgumentNullException("serverServiceFacade");

			this.serverServiceFacade = serverServiceFacade;

			this.Initialize();
		}

		private void Initialize() {
			Eve.Diagnostics.Logging.Log.OnMessage += this.OnMessage;

			// Initialize collections
			this.messages = new List<Eve.Diagnostics.Logging.Log.LogMessage>();
			this.Types = new ObservableCollection<Type>();
			this.selectedTypes = new List<Type>();

			// For testing only
			// TODO Implement save settings
			this.ShowDebugMessages = true;
			this.ShowErrorMessages = true;
			this.ShowInfoMessages = true;
			this.ShowWarnMessages = true;
			this.ShowWriteMessages = true;
			this.IsAutoScroll = true;
		}

		private void OnMessage(Eve.Diagnostics.Logging.Log.LogInstance instance,
							   Eve.Diagnostics.Logging.Log.LogMessage message) {
			// Add message to the list
			this.messages.Add(message);

			// Check groups
			if (!this.Types.Contains(message.SenderType)) {
				this.Types.Add(message.SenderType);
				this.selectedTypes.Add(message.SenderType);

				if (this.OnSelectedTypeAdded != null)
					this.OnSelectedTypeAdded(message.SenderType, null);
			}

			// Trigger messages changed
			this.RaisePropertyChanged(() => this.Messages);

			if (this.OnMessageAdded != null)
				this.OnMessageAdded(message, null);
		}

		public void UpdateSelectedTypes(IList selectedItems) {
			this.selectedTypes = selectedItems.OfType<Type>().ToList();
			this.RaisePropertyChanged(() => this.Messages);
		}

		private bool IsSelectedType(Eve.Diagnostics.Logging.Log.LogMessage message) {
			return this.selectedTypes.Contains(message.SenderType) &&
				   ((this.ShowWriteMessages &&
					 message.Level.HasFlag(Eve.Diagnostics.Logging.Log.LogLevels.Write)) ||
					(this.ShowDebugMessages &&
					 message.Level.HasFlag(Eve.Diagnostics.Logging.Log.LogLevels.Debug)) ||
					(this.ShowInfoMessages &&
					 message.Level.HasFlag(Eve.Diagnostics.Logging.Log.LogLevels.Information)) ||
					(this.ShowWarnMessages &&
					 message.Level.HasFlag(Eve.Diagnostics.Logging.Log.LogLevels.Warninig)) ||
					(this.ShowErrorMessages &&
					 message.Level.HasFlag(Eve.Diagnostics.Logging.Log.LogLevels.Error)));
		}

		#region Properties

		public IEnumerable<Eve.Diagnostics.Logging.Log.LogMessage> Messages {
			get { return this.messages.Where(this.IsSelectedType); }
		}

		public ObservableCollection<Type> Types { get; private set; }

		public string TypeSelected {
			get { return this.typeSelected; }
			set {
				this.typeSelected = value;
				this.RaisePropertyChanged(() => this.TypeSelected);
			}
		}

		public bool IsAutoScroll {
			get { return this.isAutoScroll; }
			set {
				this.isAutoScroll = value;
				this.RaisePropertyChanged(() => this.IsAutoScroll);
			}
		}

		public bool ShowDebugMessages {
			get { return this.showDebugMessages; }
			set {
				this.showDebugMessages = value;
				this.RaisePropertyChanged(() => this.ShowDebugMessages);
			}
		}

		public bool ShowErrorMessages {
			get { return this.showErrorMessages; }
			set {
				this.showErrorMessages = value;
				this.RaisePropertyChanged(() => this.ShowErrorMessages);
			}
		}

		public bool ShowWarnMessages {
			get { return this.showWarnMessages; }
			set {
				this.showWarnMessages = value;
				this.RaisePropertyChanged(() => this.ShowWarnMessages);
			}
		}

		public bool ShowInfoMessages {
			get { return this.showInfoMessages; }
			set {
				this.showInfoMessages = value;
				this.RaisePropertyChanged(() => this.ShowInfoMessages);
			}
		}

		public bool ShowWriteMessages {
			get { return this.showWriteMessages; }
			set {
				this.showWriteMessages = value;
				this.RaisePropertyChanged(() => this.ShowWriteMessages);
			}
		}

		#endregion

		#region IDisposable implementation

		private bool isDisposed;

		public void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected async virtual void Dispose(bool disposing) {
			if (this.isDisposed) return;

			if (disposing) {
				Eve.Diagnostics.Logging.Log.OnMessage -= this.OnMessage;
			}

			this.isDisposed = true;
		}

		#endregion
	}
}
