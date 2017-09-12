﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Converters
{
    [ValueConversion(typeof(string), typeof(System.Windows.FontWeight))]
    public class FontConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            FontWeight fontWt;
            switch (value.ToString())
            {
                case "Bold":
                    fontWt = FontWeights.Bold;
                    break;
                case "ExtraBold":
                    fontWt = FontWeights.ExtraBold;
                    break;
                case "Normal":
                    fontWt = FontWeights.Normal;
                    break;
                case "Light":
                    fontWt = FontWeights.Light;
                    break;
                default:
                    fontWt = FontWeights.Normal;
                    break;
            }
            return fontWt;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            FontWeight fontWt = (FontWeight)value;
            return fontWt.ToString();
        }
    }

    public class FontMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var val = value.FirstOrDefault(x => x != null);
            FontWeight fontWt;
            switch (val?.ToString())
            {
                case "Bold":
                    fontWt = FontWeights.Bold;
                    break;
                case "ExtraBold":
                    fontWt = FontWeights.ExtraBold;
                    break;
                case "Normal":
                    fontWt = FontWeights.Normal;
                    break;
                case "Light":
                    fontWt = FontWeights.Light;
                    break;
                default:
                    fontWt = FontWeights.Normal;
                    break;
            }
            return fontWt;
        }
        public object[] ConvertBack(object value, Type[] targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            FontWeight fontWt = (FontWeight)value;
            return new object[]{fontWt.ToString()};
        }
    }

    public class FontSizeMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var val = value.FirstOrDefault(x => x != null);
            var res = System.Convert.ToDouble(val);
            return res;
        }
        public object[] ConvertBack(object value, Type[] targetType, object parameter, System.Globalization.CultureInfo culture) => null;
    }
}
