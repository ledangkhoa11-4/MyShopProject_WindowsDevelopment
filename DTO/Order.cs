using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Rendering;

namespace MyShopProject.DTO
{
    public class Order: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string _id { get; set; } = "";
        public ObservableCollection<DetailOrder>BookAndQuantity { get; set; }
        public Date PurchaseDate { get; set; } = Date.Now;
        public string Customer { get; set; } = "";
    }
}
