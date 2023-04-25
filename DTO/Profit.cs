using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShopProject.DTO
{
    public class Profit : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public int profit { get; set; } = 0;
        public string time { get; set; } = "";
    }
}
