using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShopProject.Converters
{
    public class DateFormat
    {
        public static string ConvertDateFormat(string inputDate)
        {
            DateTime date;
            if (DateTime.TryParseExact(inputDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out date))
            {
                return date.ToString("MMM dd");
            }
            else
            {
                return inputDate;
            }
        }
    }
}
