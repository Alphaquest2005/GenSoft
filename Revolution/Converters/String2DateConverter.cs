using System;
using System.Globalization;
using System.Windows.Data;

namespace Converters
{
    public class String2DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime res;
            var date = value as string;
            if (date == null) return null;
            if (DateTime.TryParse(date, out res)) return res;
            return null;
        }


        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture) => System.Convert.ToString(value);
    }
}