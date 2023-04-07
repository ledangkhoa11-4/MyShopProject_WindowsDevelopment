using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Documents.FormatProviders.Html.Parsing.Dom;
using Telerik.Windows.Rendering;

namespace MyShopProject.DTO
{
    public class Order: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string _id { get; set; } = "";
        private ObservableCollection<DetailOrder> _bookAndQuantity = new ObservableCollection<DetailOrder>();
        
        public ObservableCollection<DetailOrder>DetailCart { 
            get
            {
                return _bookAndQuantity;
            }
            set {
                _bookAndQuantity= value;
                TotalPriceOrder = 0;
                foreach (DetailOrder detail in _bookAndQuantity)
                {
                    TotalPriceOrder += detail.TotalPrice;
                }
            }
        } 
        
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
        public string Customer { get; set; } = "";
        private Coupon _coupon = null;
        public Coupon Coupon { get
            {
                return _coupon;
            }
            set
            {
                _coupon = value;
                UpdateTotalPrice();
            }
        }
        public int TotalPriceOrder { get; set; } = 0;

        public Order()
        {
            _bookAndQuantity.CollectionChanged += OnItemsCollectionChanged;
        }
        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (DetailOrder item in e.NewItems)
                {
                    item.PropertyChanged += OnDetailOrderPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (DetailOrder item in e.OldItems)
                {
                    item.PropertyChanged -= OnDetailOrderPropertyChanged;
                }
            }

            UpdateTotalPrice();
        }
        
        private void OnDetailOrderPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TotalPrice")
            {
                UpdateTotalPrice();
            }
        }

        private void UpdateTotalPrice()
        {
            int total = 0;
            foreach (DetailOrder item in _bookAndQuantity)
            {
                total += item.TotalPrice;
            }
            if(this.Coupon!= null)
            {
                var discountPrice = total*Coupon.DiscountPercent/100.0;
                total = total - Convert.ToInt32(discountPrice);
            }
            TotalPriceOrder = total;
        }
    }
}
