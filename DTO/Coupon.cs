﻿using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShopProject.DTO
{
    public class Coupon : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string _id { get; set; }
        public string Name { get; set; }
        public Date DateAdd { get; set; }
        public double DiscountPercent { get; set; }
    }
}
