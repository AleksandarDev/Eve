using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EveWindowsPhone.Converters {
	public class BooleanToVisibilityConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value == null || !(value is Boolean))
				return Visibility.Visible;

			if ((bool)value) return Visibility.Visible;
			else return Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return null;
		}
	}
}