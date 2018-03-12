using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Converters
{
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
}