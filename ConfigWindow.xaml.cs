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
    /// Interaction logic for ConfigWindow.xaml
    /// </summary>
    public class ConfigModel
    {
        public int productPerPage { get; set; } = 1;
        public int orderPerPage { get; set; } = 1;
        public int totalOrders { get; set; } = 1;
        public int totalProducts { get; set; } = 1;
        public ConfigModel(int p, int o, int tp, int to)
        {
            this.orderPerPage = o;
            this.productPerPage = p;
            this.totalOrders = to;
            this.totalProducts = tp;
        }
    }
    public partial class ConfigWindow : Window
    {

        public ConfigModel model;
        public ConfigWindow(int productPerPage, int orderPerPage, int totalP, int totalO)
        {
            InitializeComponent();
            model = new ConfigModel(productPerPage, orderPerPage, totalP, totalO);
            this.DataContext = model;
        }

        private void applyConfClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
