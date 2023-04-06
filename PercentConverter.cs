﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyShopProject
{
    public class PercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringNumber = value.ToString();
            return $"{stringNumber}%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
