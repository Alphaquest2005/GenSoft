using System;
using System.Globalization;
using System.Windows.Data;
using JB.Collections.Reactive;

namespace Converters
{
    public class DisplayMemberConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            var prop = value[0] as string;
            var cachelst = value[1] as IObservableReadOnlyDictionary<string, string>;
            
            if (prop != null && cachelst != null && cachelst.ContainsKey(prop))
            {
                return cachelst[prop];
                
            }
            return null;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }
}