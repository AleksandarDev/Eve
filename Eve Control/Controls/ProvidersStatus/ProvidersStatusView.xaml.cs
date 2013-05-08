﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Eve.API;
using MahApps.Metro.Controls;

namespace EveControl.Controls.ProvidersStatus {
	/// <summary>
	/// Interaction logic for ProvidersStatusView.xaml
	/// </summary>
	public partial class ProvidersStatusView : UserControl {
		public ProvidersStatusView() {
			InitializeComponent();
		}

		private void ProvidersStatusViewOnLoaded(object sender, RoutedEventArgs e) {
			this.ProvidersStackPanel.Children.Add(
				this.CreateProviderControl(ProviderManager.TouchProvider,
					"Touch Provider"));
			this.ProvidersStackPanel.Children.Add(
				this.CreateProviderControl(ProviderManager.ProcessProvider,
					"Process Provider"));
			this.ProvidersStackPanel.Children.Add(
				this.CreateProviderControl(ProviderManager.ScriptingProvider,
					"Scripting Provider"));
			this.ProvidersStackPanel.Children.Add(
				this.CreateProviderControl(ProviderManager.SpeechProvider,
					"Speech Provider"));
			this.ProvidersStackPanel.Children.Add(
				this.CreateProviderControl(ProviderManager.TextProvider,
					"Text Provider"));
			this.ProvidersStackPanel.Children.Add(
				this.CreateProviderControl(ProviderManager.CaptchaDecoderProvider,
					"Captcha Decoder Provider"));
			this.ProvidersStackPanel.Children.Add(
				this.CreateProviderControl(ProviderManager.DisplayEnhancementsProvider,
					"Display Enhancements Provider"));
			this.ProvidersStackPanel.Children.Add(
				this.CreateProviderControl(ProviderManager.VideoProvider,
					"Video Provider"));
			this.ProvidersStackPanel.Children.Add(
				this.CreateProviderControl(ProviderManager.FaceDetectionProvider,
					"Face Detection Provider"));
			this.ProvidersStackPanel.Children.Add(
				this.CreateProviderControl(ProviderManager.ScreenViewerProvider,
					"Screen Viewer Provider"));
		}

		private UIElement CreateProviderControl(IProvider provider, string alias) {
			var root = new Grid();
			root.ColumnDefinitions.Add(new ColumnDefinition() {Width = GridLength.Auto});
			root.ColumnDefinitions.Add(new ColumnDefinition());

			var image = new BitmapImage();
			image.BeginInit();
			image.UriSource = new Uri("/Resources/Images/Tick.png",
				UriKind.Relative);
			image.DecodePixelWidth = 32;
			image.EndInit();

			var statusImage = new Image() {
				Source = image,
				Visibility = System.Windows.Visibility.Hidden
			};

			provider.OnStarted += p => statusImage.Visibility =
				p.IsRunning ? Visibility.Visible : Visibility.Hidden;
			provider.OnStopped += p => statusImage.Visibility =
				p.IsRunning ? Visibility.Visible : Visibility.Hidden;

			var providerAlias = new Label() {
				Content = alias,
				VerticalAlignment = System.Windows.VerticalAlignment.Center
			};

			Grid.SetColumn(statusImage, 0);
			Grid.SetColumn(providerAlias, 1);
			root.Children.Add(statusImage);
			root.Children.Add(providerAlias);

			return root;
		}
	}
}
