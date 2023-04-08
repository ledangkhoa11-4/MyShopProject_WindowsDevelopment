using MyShopProject.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyShopProject.Converters
{
    public class OldDiscountPriceConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var currentPriceStr = values[0] as String;
                if (values[1] == null) return "";
                var currentDiscountStr = values[1].ToString();
                currentPriceStr = currentPriceStr.Remove(currentPriceStr.Length - 1);
                currentPriceStr = currentPriceStr.Replace(".", "");
                var currentPrice = int.Parse(currentPriceStr);
                var currentDiscount = Double.Parse(currentDiscountStr);
                if (currentDiscount == 100 || currentPrice == 0)
                    return "";
                var oldPrice = currentPrice / ((100-currentDiscount) / 100);
                var priceStr = System.Convert.ToInt32(oldPrice).ToString();

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
            catch {
                return "";
            }
            
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
