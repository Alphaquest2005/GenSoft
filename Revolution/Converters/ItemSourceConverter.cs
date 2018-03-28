using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using SystemInterfaces;
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
                || !(value[2] is List<IDynamicRelationshipType> propertyParentEntityTypes) 
                || propertyParentEntityTypes.FirstOrDefault(x => x.Key == prop) == null)  return null;


            return DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(propertyParentEntityTypes.First(x => x.Key == prop).Type).CachedProperties.ContainsKey("Name")
                ? DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(propertyParentEntityTypes.First(x => x.Key == prop).Type).CachedProperties["Name"]
                : null;

        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }
}