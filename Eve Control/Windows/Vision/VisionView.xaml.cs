﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Drawing;
using AForge.Controls;
using AForge.Imaging;
using AForge.Video;
using Eve.API;
using Eve.API.Vision;
using Eve.Diagnostics.Logging;
using MahApps.Metro.Controls;
using Size = System.Drawing.Size;

namespace EveControl.Windows.Vision {
	public partial class VisionView : MetroWindow {
		private readonly Log.LogInstance log = new
			Log.LogInstance(typeof(VisionView));

		private Size videoPlayerSize = new Size(320, 240);
		private VideoSourcePlayer videoSourcePlayer;


		public VisionView() {
			InitializeComponent();
		}

		private async void VisionViewOnLoaded(object sender, RoutedEventArgs e) {
			//await this.LoadFaceDetectionProvider();
			this.LoadVideoPlayer();
			
			this.SetStatus("Ready");
		}

		//private async Task LoadFaceDetectionProvider() {
		//	this.SetStatus("Initializing face detection provider...");

		//	this.faceDetectionProvider = new FaceDetectionProvider();
		//	await this.faceDetectionProvider.InitializeAsync();
		//	await Task.Run(() => this.faceDetectionProvider.Source.Start());

		//	this.SetStatus("Face detection provider successfully initialized");
		//}

		private void LoadVideoPlayer() {
			this.SetStatus("Creating video viewer...");

			this.videoSourcePlayer = new VideoSourcePlayer() {
				Size = this.videoPlayerSize,
				VideoSource = ProviderManager.VideoProvider.DeviceVideoSource
			};
			this.videoSourcePlayer.NewFrame += HandleNewVideoFrame;
			this.VideoSourcePlayerWindowsFormsHost.Child = this.videoSourcePlayer;

			this.SetStatus("Video player successfully created");
		}

		private void HandleNewVideoFrame(object sender, ref Bitmap image) {
			if (ProviderManager.FaceDetectionProvider == null ||
				!ProviderManager.FaceDetectionProvider.IsRunning) {
				this.log.Warn("Can't process frame. Start face detection provider first");
				return;
			}

			var unmanagedImage = UnmanagedImage.FromManagedImage(image);

			foreach (var tracker in ProviderManager.FaceDetectionProvider.Trackers) {
				tracker.DrawMarker(ref unmanagedImage, true);
			}

			image = unmanagedImage.ToManagedImage();
			unmanagedImage.Dispose();
		}

		private void VisionViewClosing(object sender, CancelEventArgs e) {
			this.videoSourcePlayer.Dispose();
		}

		private void SetStatus(string message) {
			this.StatusLabel.Dispatcher.InvokeAsync(() => {
				this.StatusLabel.Content = message;
			});
		}
	}
}