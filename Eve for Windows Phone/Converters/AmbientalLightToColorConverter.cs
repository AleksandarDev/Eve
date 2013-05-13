using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using EveWindowsPhone.RelayServiceReference;

namespace EveWindowsPhone.Converters {
	public class AmbientalLightToColorConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			var light = value as AmbientalLight;
			if (light == null) return null;

			return new Color() {
				A = light.AValue,
				R = light.RValue,
				G = light.GValue,
				B = light.BValue
			};
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return null;
		}
	}
}
