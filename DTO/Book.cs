using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Rendering;

namespace MyShopProject.DTO
{
    public class Book : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string _id { get; set; }
        public string Name { get; set; }
        public string ImageBase64 { get; set; }
        public int PurchasePrice { get; set; }
        public int SellingPrice { get; set; }
        public string Author { get; set; }
        public int PublishedYear { get; set; }
        public int QuantityStock { get; set; }
        public int QuantityOrder { get; set; } = 0;
        public long CatID { get; set; }
        public string Description { get; set; }
        public bool IsOnStock { get; set; } = true;
    }
}
