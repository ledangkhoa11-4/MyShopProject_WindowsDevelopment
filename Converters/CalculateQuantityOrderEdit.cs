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
    public class CalculateQuantityOrderEdit : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var order = value as DetailOrder;
            var oldQuantityBuy = order.QuantityBuy;
            var currentQuantity = order.Book.QuantityStock;
           
            if (currentQuantity >= oldQuantityBuy)
            {

                Debug.WriteLine(currentQuantity);
                return currentQuantity;
            }
            else
            {
                Debug.WriteLine(oldQuantityBuy);
                return oldQuantityBuy;

            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
