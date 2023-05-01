using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShopProject.DTO
{
    public class StatisticsProduct : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string category { get; set; } = "";  //category là label của trục hoành chứ không phải category book
        public double quantitySelling { get; set; } = 0;

    }
}
