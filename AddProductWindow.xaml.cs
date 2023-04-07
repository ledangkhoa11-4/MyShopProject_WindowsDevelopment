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
using MyShopProject.DTO;
using System.Collections.ObjectModel;

namespace MyShopProject
{
    
    public partial class AddProductWindow : Window
    {
        public ObservableCollection<Category> _listCat { get; set; }
        public AddProductWindow(ObservableCollection<Category> listCat)
        {
            InitializeComponent();
            _listCat = listCat;
            DataContext = this;
        }

        private void RadRibbonButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Save_Product_Btn(object sender, RoutedEventArgs e)
        {
            //Write code here
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
