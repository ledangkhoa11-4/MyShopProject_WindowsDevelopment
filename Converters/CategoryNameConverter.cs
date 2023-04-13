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
    internal class CategoryNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value != null)
            {
                string catIdStr = value.ToString();
                var name = MainWindow.modelBinding.listCat.FirstOrDefault(c => c._id == catIdStr);
                if(name != null)
                 return name.Name;
                return "Null";
            }
            else return "Null";
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
