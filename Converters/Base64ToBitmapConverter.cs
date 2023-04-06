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
    public class Base64ToBitmapConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var base64Str = value as string;
            try
            {
                byte[] bytes = System.Convert.FromBase64String(base64Str);
                using (MemoryStream stream = new MemoryStream(bytes))
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = stream;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                    return image;
                }
            }
            catch(Exception ex) { 
                return new BitmapImage();
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage bitmap = value as BitmapImage;
            MemoryStream ms = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder(); // or PngBitmapEncoder
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(ms);
            byte[] imageData = ms.ToArray();
            string base64String = System.Convert.ToBase64String(imageData);
            return base64String;
        }
    }
}
