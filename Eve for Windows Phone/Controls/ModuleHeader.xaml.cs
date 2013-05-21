using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using EveWindowsPhone.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace EveWindowsPhone.Controls {
	public partial class ModuleHeader : UserControl {
		public ModuleHeader() {
			InitializeComponent();
		}


		#region Properties

		public string Alias {
			get { return (string)GetValue(AliasProperty); }
			set { SetValue(AliasProperty, value); }
		}

		public string ImageSourcePath {
			get { return (string)GetValue(ImageSourcePathProperty); }
			set { SetValue(ImageSourcePathProperty, value); }
		}

		public static readonly DependencyProperty AliasProperty =
			DependencyProperty.Register("Alias", typeof(string), typeof(ModuleHeader), new PropertyMetadata(String.Empty));

		public static readonly DependencyProperty ImageSourcePathProperty =
			DependencyProperty.Register("ImageSourcePath", typeof(string), typeof(ModuleHeader), new PropertyMetadata(String.Empty));

		#endregion
	}
}
