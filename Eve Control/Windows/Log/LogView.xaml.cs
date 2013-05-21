using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EveControl.Annotations;
using MahApps.Metro.Controls;

namespace EveControl.Windows.Log {
	/// <summary>
	/// Interaction logic for LogView.xaml
	/// </summary>
	public partial class LogView : MetroWindow, INotifyPropertyChanged {
		private List<Eve.Diagnostics.Logging.Log.LogMessage> messages; 

		public LogView() {
			this.messages =
				new List<Eve.Diagnostics.Logging.Log.LogMessage>();
			Eve.Diagnostics.Logging.Log.OnMessage +=
				(instance, message) => this.Dispatcher.InvokeAsync(() => {
					this.messages.Add(message);
					OnPropertyChanged("Messages");

					// Add to info messages
					//if (message.Level.HasFlag(Eve.Diagnostics.Logging.Log.LogLevels.Information))
					//	this.InfoMessage.Add(message);
					if (this.IsAutoScroll) {
						var selectedTab = this.LogInstances.SelectedItem as TabItem;
						if (selectedTab != null) {
							var messagesListBox = selectedTab.FindChildren<ListBox>();
							foreach (var listBox in messagesListBox) {
								if (!listBox.Items.IsEmpty)
									listBox.ScrollIntoView(listBox.Items.GetItemAt(listBox.Items.Count - 1));
							}
						}
					}
				});

			InitializeComponent();
		}


		#region Properties

		public IEnumerable<Eve.Diagnostics.Logging.Log.LogMessage> Messages {
			get {
				return this.messages.Where(m =>
										   (this.ShowWriteMessages &&
											m.Level.HasFlag(Eve.Diagnostics.Logging.Log.LogLevels.Write)) ||
										   (this.ShowDebugMessages &&
											m.Level.HasFlag(Eve.Diagnostics.Logging.Log.LogLevels.Debug)) ||
										   (this.ShowInfoMessages &&
											m.Level.HasFlag(Eve.Diagnostics.Logging.Log.LogLevels.Information)) ||
										   (this.ShowWarnMessages &&
											m.Level.HasFlag(Eve.Diagnostics.Logging.Log.LogLevels.Warninig)) ||
										   (this.ShowErrorMessages &&
											m.Level.HasFlag(Eve.Diagnostics.Logging.Log.LogLevels.Error)));
			}
		}

		public static readonly DependencyProperty ShowWriteMessagesProperty =
			DependencyProperty.Register("ShowWriteMessages", typeof(bool), typeof(LogView),
													   new PropertyMetadata(default(bool)));

		public static readonly DependencyProperty ShowInfoMessagesProperty =
			DependencyProperty.Register("ShowInfoMessages", typeof(bool), typeof(LogView),
													   new PropertyMetadata(default(bool)));

		public static readonly DependencyProperty ShowWarnMessagesProperty =
			DependencyProperty.Register("ShowWarnMessages", typeof(bool), typeof(LogView),
													   new PropertyMetadata(default(bool)));

		public static readonly DependencyProperty ShowErrorMessagesProperty =
			DependencyProperty.Register("ShowErrorMessages", typeof(bool), typeof(LogView),
													   new PropertyMetadata(default(bool)));

		public static readonly DependencyProperty ShowDebugMessagesProperty =
			DependencyProperty.Register("ShowDebugMessages", typeof(bool), typeof(LogView),
													   new PropertyMetadata(default(bool)));

		public static readonly DependencyProperty IsAutoScrollProperty =
			DependencyProperty.Register("IsAutoScroll", typeof(bool), typeof(LogView),
													   new PropertyMetadata(default(bool)));

		public bool IsAutoScroll {
			get { return (bool)GetValue(IsAutoScrollProperty); }
			set { SetValue(IsAutoScrollProperty, value); }
		}

		public bool ShowDebugMessages {
			get { return (bool)GetValue(ShowDebugMessagesProperty); }
			set { SetValue(ShowDebugMessagesProperty, value); }
		}

		public bool ShowErrorMessages {
			get { return (bool)GetValue(ShowErrorMessagesProperty); }
			set { SetValue(ShowErrorMessagesProperty, value); }
		}

		public bool ShowWarnMessages {
			get { return (bool)GetValue(ShowWarnMessagesProperty); }
			set { SetValue(ShowWarnMessagesProperty, value); }
		}

		public bool ShowInfoMessages {
			get { return (bool)GetValue(ShowInfoMessagesProperty); }
			set { SetValue(ShowInfoMessagesProperty, value); }
		}

		public bool ShowWriteMessages {
			get { return (bool)GetValue(ShowWriteMessagesProperty); }
			set { SetValue(ShowWriteMessagesProperty, value); }
		}

		#endregion

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged(
			[CallerMemberName] string propertyName = null) {
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
