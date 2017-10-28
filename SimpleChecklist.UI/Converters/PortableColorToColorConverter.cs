﻿using System;
using System.Globalization;
using SimpleChecklist.Common.Entities;
using Xamarin.Forms;

namespace SimpleChecklist.UI.Converters
{
    public class PortableColorToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var portableColor = value as PortableColor;
            if (portableColor == null) return Color.White;

            var color = Color.FromRgba(portableColor.R, portableColor.G, portableColor.B, portableColor.A);

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}