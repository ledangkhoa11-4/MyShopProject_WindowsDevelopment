using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;

namespace MyShopProject.Converters
{
    public class PriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string priceStr = value.ToString();
            int numDigits = priceStr.Length;
            string result = "";
            for (int i = 0; i < numDigits; i++)
            {
                result += priceStr[i];
                if ((numDigits - i - 1) % 3 == 0 && i != numDigits - 1)
                {
                    result += ".";
                }
            }
            result += "đ";
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
