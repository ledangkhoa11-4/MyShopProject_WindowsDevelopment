using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyShopProject.BUS;
using MyShopProject.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public class DetailViewModel
    {
        public Order order { get; set; }
        public int subTotal { get; set; } = 0;
        public DetailViewModel(Order order)
        {
            this.order = order;
        }
    }
    public partial class OrderDetailWindow : Window
    {
        public DetailViewModel model { get; set; }
        public OrderDetailWindow(Order order)
        {
            InitializeComponent();
            model = new DetailViewModel(order);
            foreach (DetailOrder cart in model.order.DetailCart)
            {
                model.subTotal += cart.TotalPrice;
            }
            this.DataContext= model;
        }

        private async void detailOrderWindowLoaded(object sender, RoutedEventArgs e)
        {
            Book_BUS bookBus = new Book_BUS();
            radBusyIndicator.IsBusy = true;
            foreach (DetailOrder cart in model.order.DetailCart)
            {
                string base64Img = await bookBus.getImageBook(cart.Book._id);
                cart.Book.ImageBase64 = base64Img;
            }
            radBusyIndicator.IsBusy = false;
        }
    }
}
