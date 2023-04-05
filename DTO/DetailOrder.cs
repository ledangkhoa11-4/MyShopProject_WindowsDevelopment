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
        public Book Book { get; set; } = null;
        private int quantityBuy;
        public int QuantityBuy {
            get { return quantityBuy; }
            set
            {
                quantityBuy = value;
                TotalPrice = quantityBuy * Book.SellingPrice;
            }
        }
        public int TotalPrice
        {
            get; set;
        }
    }
}
