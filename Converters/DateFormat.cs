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
        public static string IntToMonth(int month)
        {
            string monthRes;
            switch (month)
            {
                case 1:
                    monthRes = "January";
                    break;
                case 2:
                    monthRes = "February";
                    break;
                case 3:
                    monthRes = "March";
                    break;
                case 4:
                    monthRes = "April";
                    break;
                case 5:
                    monthRes = "May";
                    break;
                case 6:
                    monthRes = "June";
                    break;
                case 7:
                    monthRes = "July";
                    break;
                case 8:
                    monthRes = "August";
                    break;
                case 9:
                    monthRes = "September";
                    break;
                case 10:
                    monthRes = "October";
                    break;
                case 11:
                    monthRes = "November";
                    break;
                case 12:
                    monthRes = "December";
                    break;
                default:
                    monthRes = "Invalid month";
                    break;
            }

            return monthRes;
        }
    }
}
