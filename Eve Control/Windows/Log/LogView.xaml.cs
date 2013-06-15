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
	public partial class LogView : MetroWindow {
		public LogView() {
			InitializeComponent();

			// Assign ViewModel
			if (this.ViewModel == null)
				if ((this.ViewModel = this.DataContext as LogViewModel) == null)
					throw new NullReferenceException("Invalid ViewModel");

			this.ViewModel.OnSelectedTypeAdded += ViewModelOnSelectedTypeAdded;
			this.ViewModel.OnMessageAdded += ViewModelOnMessageAdded;
		}

		private void LogViewLoaded(object sender, RoutedEventArgs e) {

		}

		private void ViewModelOnMessageAdded(object sender, EventArgs e) {
			if (this.ViewModel.IsAutoScroll && this.MessagesListBox.Items.Count > 0) {
				this.MessagesListBox.Dispatcher.BeginInvoke(
					new Action(() => this.MessagesListBox.ScrollIntoView(
						this.MessagesListBox.Items.GetItemAt(
							this.MessagesListBox.Items.Count - 1))));
			}
		}

		void ViewModelOnSelectedTypeAdded(object sender, EventArgs e) {
			if (sender is Type)
				this.LogInstanceTypesListBox.SelectedItems.Add(sender);
		}

		private void SelectAllOnClick(object sender, RoutedEventArgs e) {
			this.LogInstanceTypesListBox.SelectAll();
		}

		private void DeselectAllOnClick(object sender, RoutedEventArgs e) {
			this.LogInstanceTypesListBox.SelectedItems.Clear();
		}

		private void LogInstanceTypesListBoxOnSelectionChanged(object sender, SelectionChangedEventArgs e) {
			this.ViewModel.UpdateSelectedTypes(this.LogInstanceTypesListBox.SelectedItems);
		}

		#region Properties

		public LogViewModel ViewModel { get; private set; }

		#endregion
	}
}
