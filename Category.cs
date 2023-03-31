using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Rendering;

namespace MyShopProject
{
    public class Category : INotifyDataChanged
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public event EventHandler DataChanged;
    }
}
