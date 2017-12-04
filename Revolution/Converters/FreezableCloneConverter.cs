using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Converters
{

    public class FreezableCloneConverter : IValueConverter

    {

        public static FreezableCloneConverter Instance = new FreezableCloneConverter();



        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is Freezable)

            {

                value = (value as Freezable).Clone();

            }



            return value;

        }



        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)

        {

            throw new NotSupportedException();

        }



    }
}
