using System;
using System.Globalization;
using Xamarin.Forms;

namespace SimpleChecklist.Models.Converters
{
    public class FileTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (FileType) value == FileType.Directory ? Color.FromHex("#B8D3FE") : Color.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}