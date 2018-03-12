using System;
using System.Globalization;
using System.Windows.Data;

namespace Converters
{
    public class DisplayTextConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            var key = value[0] as string;
            var text1 = value[1] as string;
            var text2 = value[2] as string;

            return (text1 ?? text2)??key;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }
}