using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using JB.Collections.Reactive;

namespace Converters
{
    public class CachedPropertyConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            var key = value[0] as string;
            var cachedProperties = value[1] as ObservableDictionary<string,List<dynamic>>;
            if(key != null && cachedProperties != null && cachedProperties.ContainsKey(key)) return cachedProperties[key];
            return null;
        }

      
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }
}