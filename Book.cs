using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Rendering;

namespace MyShopProject
{
    public class Book : INotifyDataChanged
    {
        public event EventHandler DataChanged;
        public long Id { get; set; }
        public string Name { get; set; }
        public string ImageBase64 { get; set; }
        public int PurchasePrice { get; set; }
        public int SellingPrice { get; set; }
        public string Author { get; set; }
        public int PublishedYear { get; set; }
        public int Quantity { get; set;}
        public long CatID { get; set; }
        public string Description { get; set; }
    }
}
