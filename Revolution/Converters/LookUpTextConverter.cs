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
    public class LookUpTextConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            var prop = value[0] as string;
            var cachedProperties = value[1] as ObservableDictionary<string, Dictionary<int, dynamic>>;
            var propertyParentEntityTypes = value[2] as List<IDynamicRelationshipType>;
            Int32.TryParse(value[3]?.ToString(), out var val);
            if (prop == null ||  cachedProperties == null|| propertyParentEntityTypes == null || propertyParentEntityTypes.FirstOrDefault(x => x.Key == prop) == null
            // || val == 0
            ) return null;
            var type = propertyParentEntityTypes.First(x => x.Key == prop).Type;

            var convert = DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(type).CachedProperties.ContainsKey("Name")
                          && DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(type).CachedProperties["Name"].ContainsKey(val)
                ? DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(type).CachedProperties["Name"][val]
                : val;
            return convert;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }
}