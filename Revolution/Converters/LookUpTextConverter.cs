using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using DomainUtilities;
using JB.Collections.Reactive;

namespace Converters
{
    public class LookUpTextConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            var prop = value[0] as string;
            var cachedProperties = value[1] as ObservableDictionary<string, Dictionary<int, dynamic>>;
            var propertyParentEntityTypes = value[2] as ObservableDictionary<string, string>;
            Int32.TryParse(value[3]?.ToString(), out var val);
            if (prop == null ||  cachedProperties == null|| propertyParentEntityTypes == null || !propertyParentEntityTypes.ContainsKey(prop) 
                // || val == 0
            ) return null;


            var convert = DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(propertyParentEntityTypes[prop]).CachedProperties.ContainsKey("Name")
                          && DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(propertyParentEntityTypes[prop]).CachedProperties["Name"].ContainsKey(val)
                ? DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(propertyParentEntityTypes[prop]).CachedProperties["Name"][val]
                : val;
            return convert;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }
}