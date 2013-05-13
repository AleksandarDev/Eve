using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using MahApps.Metro.Controls;

namespace EveControl.Windows.Log {
	/// <summary>
	/// Interaction logic for LogView.xaml
	/// </summary>
	public partial class LogView : MetroWindow {
		public LogView() {
			this.Messages =
				new ObservableCollection<Eve.Diagnostics.Logging.Log.LogMessage>();
			Eve.Diagnostics.Logging.Log.OnMessage +=
				(instance, message) => this.Dispatcher.InvokeAsync(() => {
					this.Messages.Add(message);
					this.LogList.ScrollIntoView(this.Messages.Last());
				});

			InitializeComponent();
		}


		#region Properties

		public ObservableCollection<Eve.Diagnostics.Logging.Log.LogMessage> Messages { get; private set; }

		#endregion
	}
}
