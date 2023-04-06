using Microsoft.OData.Edm;
using MyShopProject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyShopProject
{
    /// <summary>
    /// Interaction logic for AddCouponWindow.xaml
    /// </summary>
    public partial class AddCouponWindow : Window
    {
        public Coupon newCoupon { get; set; }
        public AddCouponWindow()
        {
            InitializeComponent();
            newCoupon = new Coupon
            {
                DateAdd= Date.Now,
                DiscountPercent = 1.0
            };
            datePicked.SelectedDate = Date.Now;
            this.DataContext = newCoupon;
        }
    }
}
