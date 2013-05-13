using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using EveWindowsPhone.RelayServiceReference;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace EveWindowsPhone.Controls {
	public partial class LightViewControl : UserControl {
		public event LightViewControlEvetnHandler OnSwitch;


		public LightViewControl() {
			InitializeComponent();
		}

		private void ToggleSwitchOnChecked(object sender, RoutedEventArgs e) {
			if (this.OnSwitch != null)
				this.OnSwitch(this);
		}


		#region Properties

		public Light Light {
			get { return (Light)GetValue(LightProperty); }
			set { SetValue(LightProperty, value); }
		}

		public static readonly DependencyProperty LightProperty =
			DependencyProperty.Register("Light", typeof(Light),
										typeof(LightViewControl),
										new PropertyMetadata(default(Light)));

		#endregion
	}
}
