using System;
using System.Globalization;
using Xamarin.Forms;

namespace SimpleChecklist.Models.Converters
{
    public class FileTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (FileType) value == FileType.Directory
                ? (Color) Application.Current.Resources["FileColor"]
                : (Color) Application.Current.Resources["DirectoryColor"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}