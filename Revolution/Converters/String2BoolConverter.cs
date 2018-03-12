using System;
using System.Globalization;
using System.Windows.Data;

namespace Converters
{
    public class String2BoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value?.ToString())) return false;
            if(value.ToString().Length == 1 && !"0,1".ToLower().Contains(value.ToString().ToLower())) return false;
            if (!"True,False,0,1".ToLower().Contains(value.ToString().ToLower())) return false;
            if ("0,1".ToLower().Contains(value.ToString().ToLower())) return System.Convert.ToBoolean(System.Convert.ToInt16(value));
            return System.Convert.ToBoolean(value);
        }


        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture) => System.Convert.ToString(value);
    }
}