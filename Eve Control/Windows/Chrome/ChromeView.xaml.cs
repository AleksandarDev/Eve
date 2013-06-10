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
using MahApps.Metro.Controls;

namespace EveControl.Windows.Chrome {
	/// <summary>
	/// Interaction logic for ChromeView.xaml
	/// </summary>
	public partial class ChromeView : MetroWindow {
		public ChromeView() {
			InitializeComponent();

			ProviderManager.ChromeProvider.OnClientConnected += ChromeProviderOnOnClientConnected;
		}

		private void ChromeProviderOnOnClientConnected(ChromeProvider provider, ChromeProviderClientEventArgs args) {
			this.ClientsListBox.Items.Add(String.Format("{0}:{1}",
														args.ClientChanged.ConnectionInfo.ClientIpAddress,
														args.ClientChanged.ConnectionInfo.ClientPort));
		}
	}
}
