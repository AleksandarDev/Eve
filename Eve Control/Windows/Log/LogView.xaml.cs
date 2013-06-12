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
		private List<string> selectedTypes;

		public LogView() {
			this.messages =
				new List<Eve.Diagnostics.Logging.Log.LogMessage>();
			this.selectedTypes = new List<string>();

			// For testing only
			// TODO Implement save settings
			this.ShowDebugMessages = true;
			this.ShowErrorMessages = true;
			this.ShowInfoMessages = true;
			this.ShowWarnMessages = true;
			this.ShowWriteMessages = true;
			this.IsAutoScroll = true;

			Eve.Diagnostics.Logging.Log.OnMessage += this.OnMessage;

			InitializeComponent();
		}

		~LogView() {
			Eve.Diagnostics.Logging.Log.OnMessage -= this.OnMessage;
		}

		private void LogViewLoaded(object sender, RoutedEventArgs e) {
			
		}

		private void OnMessage(Eve.Diagnostics.Logging.Log.LogInstance instance,
							   Eve.Diagnostics.Logging.Log.LogMessage message) {
			this.Dispatcher.InvokeAsync(() => {
				// Add message to the list
				this.messages.Add(message);

				// Check groups
				bool typeSaved = false;
				for (int index = 0; index < this.LogInstanceTypesListBox.Items.Count; index++) {
					var listBoxItem = this.LogInstanceTypesListBox.Items.GetItemAt(index) as ListBoxItem;
					if (listBoxItem == null) continue;

					if (listBoxItem.Content.ToString() == message.SenderType.ToString()) {
						typeSaved = true;
						break;
					}
				}
				if (!typeSaved) {
					this.LogInstanceTypesListBox.Items.Add(new ListBoxItem() {
						Content = message.SenderType
					});
				}

				// Trigger messages changed
				OnPropertyChanged("Messages");

				// Auto scroll
				if (this.IsAutoScroll && this.MessagesListBox.Items.Count > 0) {
					this.MessagesListBox.ScrollIntoView(
						this.MessagesListBox.Items.GetItemAt(
							this.MessagesListBox.Items.Count - 1));
				}
			});
		}

		private void LogInstanceTypesTreeView_OnSelected(object sender, RoutedEventArgs e) {
			this.selectedTypes.Clear();
			foreach (var selectedItem in this.LogInstanceTypesListBox.SelectedItems) {
				this.selectedTypes.Add((selectedItem as ListBoxItem).Content.ToString());
			}

			// Change header
			if (this.selectedTypes.Count == 0) this.TypeSelected = "All";
			else if (this.selectedTypes.Count == 1)
				this.TypeSelected = this.selectedTypes.First();
			else this.TypeSelected = "*multiple";

			// Trigger messages changed
			OnPropertyChanged("Messages");
		}


		private void SelectAllOnClick(object sender, RoutedEventArgs e) {
			this.LogInstanceTypesListBox.SelectAll();
		}

		private void DeselectAllOnClick(object sender, RoutedEventArgs e) {
			this.LogInstanceTypesListBox.SelectedItems.Clear();
		}

		#region Properties

		public IEnumerable<Eve.Diagnostics.Logging.Log.LogMessage> Messages {
			get {
				return
					this.messages.Where(
						m =>
						(this.selectedTypes.Count == 0 ||
						 this.selectedTypes.Contains(m.SenderType.ToString()) &&
						((this.ShowWriteMessages &&
						 m.Level.HasFlag(Eve.Diagnostics.Logging.Log.LogLevels.Write)) ||
						(this.ShowDebugMessages &&
						 m.Level.HasFlag(Eve.Diagnostics.Logging.Log.LogLevels.Debug)) ||
						(this.ShowInfoMessages &&
						 m.Level.HasFlag(Eve.Diagnostics.Logging.Log.LogLevels.Information)) ||
						(this.ShowWarnMessages &&
						 m.Level.HasFlag(Eve.Diagnostics.Logging.Log.LogLevels.Warninig)) ||
						(this.ShowErrorMessages &&
						 m.Level.HasFlag(Eve.Diagnostics.Logging.Log.LogLevels.Error)))));
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

		public static readonly DependencyProperty TypeSelectedProperty =
			DependencyProperty.Register("TypeSelected", typeof(string), typeof(LogView),
													   new PropertyMetadata(default(string)));

		public string TypeSelected {
			get { return (string)GetValue(TypeSelectedProperty); }
			set { SetValue(TypeSelectedProperty, value); }
		}

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
