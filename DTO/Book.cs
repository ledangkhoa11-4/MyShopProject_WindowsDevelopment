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
        private int _quantityStock;
        public int QuantityStock
        {
            get
            {
                return _quantityStock;
            }
            set
            {
                _quantityStock= value;
                if(QuantityStock <= 0)
                {
                    isOnStock = false;
                }
                else
                {
                    isOnStock = true;
                }
            }
        }
        public int QuantityOrder { get; set; } = 0;
        public string CatID { get; set; }
        public string Description { get; set; }
        private bool isOnStock;
        public bool IsOnStock
        {
            get
            {
                return isOnStock;
            }
            set
            {
                isOnStock = value;
                if (QuantityStock == 0)
                {
                    isOnStock = false;
                }
            }
        } 
    }
}
