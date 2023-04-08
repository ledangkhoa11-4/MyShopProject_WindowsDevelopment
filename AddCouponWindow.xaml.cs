using Microsoft.OData.Edm;
using MyShopProject.BUS;
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
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams;

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
                Name = "",
                DateAdd= Date.Now,
                DiscountPercent = 1.0
            };
            datePicked.SelectedDate = Date.Now;
            this.DataContext = newCoupon;
        }

        private async void createCouponClick(object sender, RoutedEventArgs e)
        { 
            if (newCoupon.Name.Length == 0)
            {
                var alert = new RadDesktopAlert();
                alert.Header = "MISSING INFORMATION";
                alert.Content = "Please enter full information of coupon before upload!!!";
                alert.ShowDuration = 3000;
                RadDesktopAlertManager manager = new RadDesktopAlertManager();
                manager.ShowAlert(alert);
            }
            else
            {
                MainWindow.modelBinding.listCoupon.Add(newCoupon);
                var coupon_BUS = new Coupon_BUS();
                var result = await coupon_BUS.AddCoupon(newCoupon);
                var alert = new RadDesktopAlert();
                if (result.Length != 0)
                {
                    alert.Header = "ADD COUPON SUCCESSFULLy";
                    alert.Content = "Congratulation, you coupon was added!!!";
                    alert.ShowDuration = 3000;
                    this.DialogResult = true;
                }
                else
                {
                    alert.Header = "ERROR";
                    alert.Content = "There was an error on update database, please try again!!!";
                    alert.ShowDuration = 3000;
                }
                RadDesktopAlertManager manager = new RadDesktopAlertManager();
                manager.ShowAlert(alert);
                
                this.Close();
            }
        }
    }
}
