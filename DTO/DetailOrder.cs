using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Rendering;

namespace MyShopProject.DTO
{
    public class DetailOrder: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Book _book;
        public Book Book 
        { get
            {
                return _book;
            }

            set { 
                _book = value;
                Price = _book.PurchasePrice;
            } 
        }
        public int Price { get; set; } 
        private int quantityBuy;
        public int QuantityBuy {
            get { return quantityBuy; }
            set
            {
                quantityBuy = value;
                TotalPrice = quantityBuy * Price;
            }
        }
        public int TotalPrice
        {
            get; set;
        }
    }
}
