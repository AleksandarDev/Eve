using System;
using System.ComponentModel;
using System.Linq.Expressions;
using EveWindowsPhone.Pages.Main;

namespace EveWindowsPhone.ViewModels {
	public class NotificationObject : INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;


		protected virtual void RaisePropertyChanged(string propertyName) {
			if (this.PropertyChanged != null)
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		protected void RaisePropertyChanged(params string[] propertyNames) {
			if (propertyNames == null) throw new ArgumentNullException("propertyNames");

			foreach (var name in propertyNames) {
				this.RaisePropertyChanged(name);
			}
		}

		protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression) {
			this.RaisePropertyChanged(PropertySupport.ExtractPropertyName(propertyExpression));
		}
	}
}