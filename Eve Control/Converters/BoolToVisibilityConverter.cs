/******************************************************************************
 * Author:	Aleksandar Toplek
 * Date:	11.04.2013.
 * Project: Eve
 * 
 * BoolToVisibilityConverter.cs
 *****************************************************************************/

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EveControl.Converters {
	public class BoolToVisibilityConverter : IValueConverter {
		public BoolToVisibilityConverter() {
			this.DefaultHidden = Visibility.Hidden;
		}


		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (!(value is Boolean))
				return "Invalid value type or null";

			return ((Boolean) value) ? Visibility.Visible : this.DefaultHidden;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			if (!(value is Visibility))
				return "Invalid value type or null";

			return ((Visibility) value) == Visibility.Visible;
		}


		#region Properties

		public Visibility DefaultHidden { get; set; }

		#endregion
	}
}
