using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using JB.Collections.Reactive;
using Utilities;
using ViewModel.Interfaces;

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
            try
            {
                Visibility res = Visibility.Collapsed;
                var controlType = value.FirstOrDefault();


                if (parameter == null)
                {
                    var val = value.FirstOrDefault(x => x != null);
                    if (val != null) Enum.TryParse(val.ToString(), out res);
                }
                else
                {
                    if (controlType != null && controlType?.ToString() == parameter.ToString())
                    {
                        var val = value.FirstOrDefault(
                            x => x != null && "Collapsed,Hidden,Visible".Contains(x.ToString()));
                        if (val != null) Enum.TryParse(val.ToString(), out res);
                    }
                    else if (parameter != null && controlType == null &&
                             value.Any(x => x?.ToString() == parameter.ToString()))
                    {
                        var val = value.FirstOrDefault(
                            x => x != null && "Collapsed,Hidden,Visible".Contains(x.ToString()));
                        if (val != null) Enum.TryParse(val.ToString(), out res);
                    }
                }


                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }

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

    public class ItemSourceConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            var prop = value[0] as string;
            var cachelst = value[1] as IObservableDictionary<string, string>;
            var cachedProperties = value[2] as IObservableList<IViewModel>;
            if (prop != null && cachelst != null && cachedProperties != null && cachelst.ContainsKey(prop))
            {
                var cache = cachedProperties.Cast<ICacheViewModel>().FirstOrDefault(x => x.ViewInfo.EntityType.Name == cachelst[prop]);
                if (cache != null) return cache.EntitySet.Value;
            }
            return null;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }

    public class DisplayMemberConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            var prop = value[0] as string;
            var cachelst = value[1] as IObservableDictionary<string, string>;
            
            if (prop != null && cachelst != null && cachelst.ContainsKey(prop))
            {
                return cachelst[prop];
                
            }
            return null;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }

    public class LookUpTextConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            var prop = value[0] as string;
            var cachelst = value[1] as IObservableDictionary<string, string>;
            var cachedProperties = value[2] as IObservableList<IViewModel>;
            var val = value[3] as int? ?? 0;
            if (prop == null || cachelst == null || cachedProperties == null || !cachelst.ContainsKey(prop) ||
                val == 0) return null;

            var cache = cachedProperties.Cast<ICacheViewModel>().FirstOrDefault(x => x.ViewInfo.EntityType.Name == cachelst[prop]);
            return cache != null ? cache.EntitySet.Value.FirstOrDefault(x => x.Id == val)?.Properties["Name"] : null;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }

    public class String2BoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value?.ToString())) return false;
            if (!"True,False,0,1".ToLower().Contains(value.ToString().ToLower())) return false;
            if ("0,1".ToLower().Contains(value.ToString().ToLower())) return System.Convert.ToBoolean(System.Convert.ToInt16(value));
            return System.Convert.ToBoolean(value);
        }


        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture) => System.Convert.ToString(value);
    }

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
