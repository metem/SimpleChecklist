using System;
using System.Globalization;
using Xamarin.Forms;

namespace SimpleChecklist.UI.Converters
{
    public class PortableColorToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string portableColor)) return Color.White;

            var color = Color.FromHex(portableColor);

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}