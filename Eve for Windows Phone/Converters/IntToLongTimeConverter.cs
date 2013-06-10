using System;
using System.Globalization;
using System.Windows.Data;

namespace EveWindowsPhone.Converters {
	public class IntToLongTimeConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			const string EmptyTime = "00:00:00";

			if (value == null) return EmptyTime;

			int intTime;
			if (!Int32.TryParse(value.ToString(), out intTime) || intTime == 0)
				return EmptyTime;

			int hours = intTime / 3600;
			int minutes = (intTime - hours * 3600) / 60;
			int seconds = intTime - hours * 3600 - minutes * 60;
			return String.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return null;
		}
	}
}