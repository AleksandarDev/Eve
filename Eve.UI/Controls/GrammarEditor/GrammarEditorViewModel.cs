using System;
using System.Windows;

namespace Eve.UI.Controls.GrammarEditor {
	public class GrammarEditorViewModel : DependencyObject {
		private Uri loadedGrammarUri;


		public GrammarEditorViewModel() {
			this.loadedGrammarUri = null;
		}
	}
}
