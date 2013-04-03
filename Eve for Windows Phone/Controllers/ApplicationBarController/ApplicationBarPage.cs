using System;
using System.Collections.Generic;
using Microsoft.Phone.Shell;

namespace EveWindowsPhone.Controllers.ApplicationBarController {
	public class ApplicationBarPage {
		public event ApplicationBarPageEventHandler OnIconButtonAdded;
		public event ApplicationBarPageEventHandler OnMenuItemAdded;
		public event ApplicationBarPageEventHandler OnModeChanged;

		private ApplicationBarMode mode;


		public ApplicationBarPage(ApplicationBarMode mode = ApplicationBarMode.Minimized) {
			this.Mode = mode;
			this.Buttons = new List<ApplicationBarIconButton>();
			this.MenuItems = new List<ApplicationBarMenuItem>();
		}


		public void AddIconButton(ApplicationBarIconButton iconButton) {
			this.Buttons.Add(iconButton);

			if (this.OnIconButtonAdded != null)
				this.OnIconButtonAdded(this);
		}
		public void AddIconButton(string text, Uri icon, Action clickAction) {
			var button = new ApplicationBarIconButton(icon) {Text = text};

			if (clickAction != null)
				button.Click += (s, e) => clickAction.Invoke();

			this.AddIconButton(button);
		}


		public void AddMenuItem(ApplicationBarMenuItem menuItem) {
			this.MenuItems.Add(menuItem);

			if (this.OnMenuItemAdded != null)
				this.OnMenuItemAdded(this);
		}
		public void AddMenuItem(string text, Action clickAction) {
			var menuItem = new ApplicationBarMenuItem(text);

			if (clickAction != null)
				menuItem.Click += (s, e) => clickAction.Invoke();

			this.AddMenuItem(menuItem);
		}

		public void Clear() {
			this.Buttons.Clear();
			this.MenuItems.Clear();
		}


		#region Properties

		public List<ApplicationBarMenuItem> MenuItems { get; private set; }

		public List<ApplicationBarIconButton> Buttons { get; private set; }

		public ApplicationBarMode Mode {
			get { return this.mode; }
			set {
				this.mode = value;
				if (this.OnModeChanged != null)
					this.OnModeChanged(this);
			}
		}

		#endregion
	}
}