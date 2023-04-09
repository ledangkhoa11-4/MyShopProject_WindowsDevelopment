using MyShopProject.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyShopProject.Converters
{
    public class CalculateAmountPurchaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ordersCart = value as ObservableCollection<DetailOrder>;
            int amountPurchase = 0;
            foreach(DetailOrder order in ordersCart)
            {
                amountPurchase += order.QuantityBuy;
            }
            if(amountPurchase<=1)
                return $"{amountPurchase} (Book) ";
            return $"{amountPurchase} (Books) ";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
