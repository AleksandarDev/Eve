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
