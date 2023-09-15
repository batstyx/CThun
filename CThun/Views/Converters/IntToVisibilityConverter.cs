using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CThun.Views.Converters
{
    public class IntToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is int v ? v != 0 ? Visibility.Visible : Visibility.Collapsed : (object)null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
