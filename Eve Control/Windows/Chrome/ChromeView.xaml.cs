using System;
using System.Windows;
using ICSharpCode.AvalonEdit.Highlighting;
using MahApps.Metro.Controls;

namespace EveControl.Windows.Chrome {
	public partial class ChromeView : MetroWindow {
		public ChromeView() {
			InitializeComponent();

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
    	}
	}
}
