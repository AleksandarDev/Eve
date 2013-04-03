using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Phone.Shell;

namespace EveWindowsPhone.Controllers.ApplicationBarController {
	public class ApplicationBarController : DependencyObject {
		private IApplicationBar bar;
		private ApplicationBarPageCollection pages;
		private string activePageKey = String.Empty;


		public ApplicationBarController(IApplicationBar bar = null) {
			this.bar = bar;
			this.pages = new ApplicationBarPageCollection();
		}



		public void SetApplicationBar(IApplicationBar applicationBar) {
			this.bar = applicationBar;
		}

		public bool AddPage(string name, ApplicationBarPage page) {
			return this.pages.Add(name, page);
		}

		public void Clear() {
			this.ClearBar();
			this.pages.Clear();
		}

		public void SetPageBar(string name = null) {
			if (this.bar == null) throw new NullReferenceException("No ApplicationBar specified");

			// Check if change is needed
			if (this.activePageKey == name) {
				System.Diagnostics.Debug.WriteLine("Already on right page");
				return;
			}
			else this.activePageKey = name;

			// Clear current bar items
			this.ClearBar();
			if (String.IsNullOrEmpty(name) || !this.pages.Contains(name))
				return;

			// Get selected page
			var page = this.pages[name];

			// Set bar to visible
			this.bar.IsVisible = true;

			// Set bar mode and attach on change event action
			this.bar.Mode = page.Mode;
			page.OnModeChanged += p => this.bar.Mode = p.Mode;

			// Populate bar with currently available buttons and menu items
			foreach (var button in page.Buttons) 
				this.bar.Buttons.Add(button);
			foreach (var menuItem in page.MenuItems)
				this.bar.MenuItems.Add(menuItem);

			// Attach on added event actions for buttons and menu items
			page.OnIconButtonAdded += p => this.bar.Buttons.Add(p.Buttons[p.Buttons.Count - 1]);
			page.OnMenuItemAdded += p => this.bar.MenuItems.Add(p.MenuItems[p.MenuItems.Count - 1]);
		}

		private void ClearBar() {
			if (this.bar == null) {
				System.Diagnostics.Debug.WriteLine("ApplicationBarController is not initialized yet");
				return;
			}

			this.bar.Buttons.Clear();
			this.bar.MenuItems.Clear();
			this.bar.IsVisible = false;
		}


		#region Properties

		public ApplicationBarPageCollection PageCollection {
			get { return this.pages; }
		}

		#endregion
	}
}
