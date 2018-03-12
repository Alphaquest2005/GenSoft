using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using DomainUtilities;
using JB.Collections.Reactive;

namespace Converters
{
    public class ItemSourceConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value[0] is string prop) 
                || !(value[1] is ObservableDictionary<string, Dictionary<int, dynamic>> cachedProperties) 
                || !(value[2] is ObservableDictionary<string, string> propertyParentEntityTypes) 
                || !propertyParentEntityTypes.ContainsKey(prop) ) return null;


            return DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(propertyParentEntityTypes[prop]).CachedProperties.ContainsKey("Name")
                ? DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(propertyParentEntityTypes[prop]).CachedProperties["Name"]
                : null;

        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }
}