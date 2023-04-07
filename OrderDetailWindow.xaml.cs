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
        public DetailViewModel(Order order)
        {
            this.order = order;
        }
    }
    public partial class OrderDetailWindow : Window
    {
        public OrderDetailWindow(Order order)
        {
            InitializeComponent();
            var model = new DetailViewModel(order);
            Debug.WriteLine(model.order.DetailCart[1].Book.Name);
            this.DataContext= model;
        }
    }
}
