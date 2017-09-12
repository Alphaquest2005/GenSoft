using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Converters
{
    public class StringVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility res = Visibility.Collapsed;
            if(value != null) Enum.TryParse(value.ToString(),out res);
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }

    public class StringVisibilityMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility res = Visibility.Collapsed;
            var val = value.FirstOrDefault(x => x != null);
            if (val != null) Enum.TryParse(val.ToString(), out res);
            return res;
        }

      
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }


}
