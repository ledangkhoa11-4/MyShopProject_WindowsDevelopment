using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Rendering;

namespace MyShopProject.DTO
{
    public class Category : INotifyDataChanged
    {
        public string _id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public event EventHandler DataChanged;
    }
}
