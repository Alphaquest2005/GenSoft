using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Converters
{
    public class StringVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility res = Visibility.Collapsed;
            var t = value != null && Enum.TryParse(value.ToString(),out res);
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }


}
