﻿using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Eve.Diagnostics.Logging;
using EveWindowsPhone.Adapters;
using EveWindowsPhone.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace EveWindowsPhone {
	public partial class App : Application {
		// Default setting values
		protected const int DefaultFavoriteRows = 2;
		protected const bool DefaultActivateZoomOnTouch = true;
		protected const int DefaultActivateZoomOnTouchValue = 200;
		protected const bool DefaultActivateZoomOnKeyboard = true;
		protected const int DefaultActivateZoomOnKeyboardValue = 200;
		protected const int DefaultLightRefreshRate = 1000;

		/// <summary>
		/// Provides easy access to the root frame of the Phone Application.
		/// </summary>
		/// <returns>The root frame of the Phone Application.</returns>
		public PhoneApplicationFrame RootFrame { get; private set; }

		/// <summary>
		/// Constructor for the Application object.
		/// </summary>
		public App() {
			// Global handler for uncaught exceptions. 
			UnhandledException += Application_UnhandledException;

			// Standard Silverlight initialization
			InitializeComponent();

			// Phone-specific initialization
			InitializePhoneApplication();

			// Change phone color theme to dark
			// TODO Optimize all modules and pages for multi color
			// TODO Remove this after optimization
			ThemeManager.ToLightTheme();

			// Show graphics profiling information while debugging.
			if (System.Diagnostics.Debugger.IsAttached) {
				// Display the current frame rate counters.
				//Application.Current.Host.Settings.EnableFrameRateCounter = true;

				// Show the areas of the app that are being redrawn in each frame.
				//Application.Current.Host.Settings.EnableRedrawRegions = true;

				// Enable non-production analysis visualization mode, h
				// which shows areas of a page that are handed off to GPU with a colored overlay.
				//Application.Current.Host.Settings.EnableCacheVisualization = true;

				// Disable the application idle detection by setting the UserIdleDetectionMode property of the
				// application's PhoneApplicationService object to Disabled.
				// Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
				// and consume battery power when the user is not using the phone.
				PhoneApplicationService.Current.UserIdleDetectionMode =
					IdleDetectionMode.Disabled;
			}

#if DEBUG
			// Enable logging
			Log.Enabled = true;
			Log.WriteToDebug = true;
			Log.WriteToDebugLevel = Log.LogLevels.All;
#endif

			// TODO Tap'n'Hold context menu to enable user to pin their favorite modules to start screen
			//ShellTile.Create(new Uri("/Pages/Modules/Lights/LightsView.xaml", UriKind.Relative),
			//				 new StandardTileData() {
			//					 Title = "Lights",
			//					 BackgroundImage = new Uri("/Resources/Images/Light Bulb.png", UriKind.Relative)
			//				 });
			//ShellTile.Create(new Uri("/Pages/Modules/Play/PlayView.xaml", UriKind.Relative),
			//				 new StandardTileData() {
			//					 Title = "Play",
			//					 BackgroundImage = new Uri("/Resources/Images/IPod.png", UriKind.Relative)
			//				 });
			//ShellTile.Create(new Uri("/Pages/Modules/Keyboard/KeyboardView.xaml", UriKind.Relative),
			//				 new StandardTileData() {
			//					 Title = "Keyboard",
			//					 BackgroundImage = new Uri("/Resources/Images/Keyboard.png", UriKind.Relative)
			//				 });
		}

		// Code to execute when the application is launching (eg, from Start)
		// This code will not execute when the application is reactivated
		private void Application_Launching(object sender, LaunchingEventArgs e) {
			// Set default application settings
			this.SetDefaultSettings();
		}

		private void SetDefaultSettings() {
			var isolatedStorageServiceFacade = new IsolatedStorageServiceFacade();

			// TODO Settings to seperate class
			// Client settings
			isolatedStorageServiceFacade.SetDefault(
				String.Empty,
				IsolatedStorageServiceFacade.ClientIDKey);

			// Favorites settings
			isolatedStorageServiceFacade.SetDefault(
				DefaultFavoriteRows,
				IsolatedStorageServiceFacade.FavoriteRowsKey);

			// Touch settings
			isolatedStorageServiceFacade.SetDefault(
				DefaultActivateZoomOnTouch,
				IsolatedStorageServiceFacade.ActivateZoomOnTouchKey);

			isolatedStorageServiceFacade.SetDefault(
				DefaultActivateZoomOnTouchValue,
				IsolatedStorageServiceFacade.ActivateZoomOnTouchValueKey);

			// Keyboard settings
			isolatedStorageServiceFacade.SetDefault(
				DefaultActivateZoomOnKeyboard,
				IsolatedStorageServiceFacade.ActivateZoomOnKeyboardKey);

			isolatedStorageServiceFacade.SetDefault(
				DefaultActivateZoomOnKeyboardValue,
				IsolatedStorageServiceFacade.ActivateZoomOnKeyboardValueKey);

			// Lights settings
			isolatedStorageServiceFacade.SetDefault(
				DefaultLightRefreshRate,
				IsolatedStorageServiceFacade.LightsRefreshRateKey);

			// Ambiental settings
			isolatedStorageServiceFacade.SetDefault(
				DefaultLightRefreshRate,
				IsolatedStorageServiceFacade.AmbientalRefreshRateKey);
		}

		// Code to execute when the application is activated (brought to foreground)
		// This code will not execute when the application is first launched
		private void Application_Activated(object sender, ActivatedEventArgs e) {}

		// Code to execute when the application is deactivated (sent to background)
		// This code will not execute when the application is closing
		private void Application_Deactivated(object sender, DeactivatedEventArgs e) {}

		// Code to execute when the application is closing (eg, user hit Back)
		// This code will not execute when the application is deactivated
		private void Application_Closing(object sender, ClosingEventArgs e) {
			// Dispose ViewModels
			var viewModelLocator = this.Resources["ViewModelLocator"] as ViewModelLocator;
			if (viewModelLocator != null) viewModelLocator.Dispose();

			// Close connection to relay service
			//EveWindowsPhone.Pages.Main.MainView.client.CloseAsync();
		}

		// Code to execute if a navigation fails
		private void RootFrame_NavigationFailed(object sender,
			NavigationFailedEventArgs e) {
			if (System.Diagnostics.Debugger.IsAttached) {
				// A navigation has failed; break into the debugger
				System.Diagnostics.Debugger.Break();
			}
		}

		// Code to execute on Unhandled Exceptions
		private void Application_UnhandledException(object sender,
			ApplicationUnhandledExceptionEventArgs e) {
			if (System.Diagnostics.Debugger.IsAttached) {
				// An unhandled exception has occurred; break into the debugger
				System.Diagnostics.Debugger.Break();
			}
		}

		#region Phone application initialization

		// Avoid double-initialization
		private bool phoneApplicationInitialized = false;

		// Do not add any additional code to this method
		private void InitializePhoneApplication() {
			if (phoneApplicationInitialized)
				return;

			// Create the frame but don't set it as RootVisual yet; this allows the splash
			// screen to remain active until the application is ready to render.
			// NOTE http://www.geekchamp.com/articles/windows-phone-7-navigation-transitions-step-by-step-guide
			RootFrame = new TransitionFrame();
			RootFrame.Navigated += CompleteInitializePhoneApplication;

			// Handle navigation failures
			RootFrame.NavigationFailed += RootFrame_NavigationFailed;

			// Ensure we don't initialize again
			phoneApplicationInitialized = true;
		}

		// Do not add any additional code to this method
		private void CompleteInitializePhoneApplication(object sender,
			NavigationEventArgs e) {
			// Set the root visual to allow the application to render
			if (RootVisual != RootFrame)
				RootVisual = RootFrame;

			// Remove this handler since it is no longer needed
			RootFrame.Navigated -= CompleteInitializePhoneApplication;
		}

		#endregion
	}
}