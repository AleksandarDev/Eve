using System;
using System.Collections.Generic;
using System.Linq;
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
using Eve.API;
using Eve.API.Chrome;
using Fleck2.Interfaces;
<<<<<<< HEAD
using MahApps.Metro.Controls;

namespace EveControl.Windows.Chrome {
	/// <summary>
	/// Interaction logic for ChromeView.xaml
	/// </summary>
=======
using ICSharpCode.AvalonEdit.Highlighting;
using MahApps.Metro.Controls;

namespace EveControl.Windows.Chrome {
>>>>>>> master
	public partial class ChromeView : MetroWindow {
		public ChromeView() {
			InitializeComponent();

<<<<<<< HEAD
			ProviderManager.ChromeProvider.OnClientConnected +=
				this.ChromeProviderOnOnClientConnectionChanged;
			ProviderManager.ChromeProvider.OnClientDisconnected +=
				this.ChromeProviderOnOnClientConnectionChanged;
		}

		private void ChromeProviderOnOnClientConnectionChanged(
			ChromeProvider provider,
			ChromeProviderClientEventArgs args) {
			this.UpdateClientsListBox(args.Clients);
		}

		private void UpdateClientsListBox(
			IEnumerable<IWebSocketConnection> connections) {
			// Call this methos with dispatcher if not called that way
			if (!this.Dispatcher.CheckAccess()) {
				this.Dispatcher.Invoke(() => UpdateClientsListBox(connections));
				return;
			}

			// Clear list
			this.ClientsListBox.Items.Clear();

			// Add connections to list
			foreach (var connection in connections) {
				this.ClientsListBox.Items.Add(connection);
			}
		}

		private void ExecuteScriptOnClick(object sender, RoutedEventArgs e) {
			ProviderManager.ChromeProvider.Push(
				this.ScriptTextBox.Text
				.Replace('\n', ' ')
				.Replace('\t'.ToString(), "   ")
				.Replace('\r', ' ')
				.Replace('\v', ' '));
=======
			// Assign ViewModel
			if (this.ViewModel == null)
				if ((this.ViewModel = this.DataContext as ChromeViewModel) == null)
					throw new NullReferenceException("Invalid ViewModel");
		}

		private void ExecuteScriptOnClick(object sender, RoutedEventArgs e) {
			this.ViewModel.ExecuteScript(this.ScriptEditor.Text);
		}

		#region Properties

		public ChromeViewModel ViewModel { get; private set; }

		#endregion

		private void ChromeViewOnLoaded(object sender, RoutedEventArgs e) {
			this.ScriptEditor.SyntaxHighlighting =
				HighlightingManager.Instance.GetDefinition("JavaScript");
>>>>>>> master
		}
	}
}
