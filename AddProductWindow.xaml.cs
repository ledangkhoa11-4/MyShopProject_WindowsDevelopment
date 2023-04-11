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
using Microsoft.Win32;
using System.IO;
using MyShopProject.Converters;
using SharpDX;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.FormatProviders.Xaml;
using Telerik.Windows.Documents.Flow.Model;
using MyShopProject.BUS;
using Telerik.Windows.Controls;

namespace MyShopProject
{
    
    public partial class AddProductWindow : Window
    {
        public ObservableCollection<DTO.Category> _listCat { get; set; }
        public Book _book { get; set; }
        public AddProductWindow(ObservableCollection<DTO.Category> listCat)
        {
            InitializeComponent();
            _book= new Book();
            _listCat = listCat;
            DataContext = this;
        }

        private void RadRibbonButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void Save_Product_Btn(object sender, RoutedEventArgs e)
        {
            _book.Name = name.Text;
            _book.Author = author.Text;
            int _purchasePrice;
            if (int.TryParse(purchasePrice.Text, out _purchasePrice))
            {
                _book.PurchasePrice = _purchasePrice;
            }
            int _sellingPrice;
            if (int.TryParse(sellingPrice.Text, out _sellingPrice))
            {
                _book.SellingPrice = _sellingPrice;
            }
            int _publishedYear;
            if (int.TryParse(publishedYear.Text, out _publishedYear))
            {
                _book.PublishedYear = _publishedYear;
            }
            try
            {
                _book.QuantityStock = (int)quantity.Value;
            }
            catch (Exception ex)
            {
                _book.QuantityStock = 1;
            }
            try
            {
                DTO.Category selectedCategory = (DTO.Category)cateChoosingBox.SelectedItem;
                if (selectedCategory != null)
                {
                    _book.CatID = selectedCategory._id;
                }
                else
                {
                    _book.CatID = _listCat[0]._id;
                }
            }
            catch (Exception ex)
            {
                _book.CatID = _listCat[0]._id;
            }


            // get the book description
            _book.Description = description.Text;
            if (_book.Name.Length == 0)
            {
                var alert = new RadDesktopAlert();
                alert.Header = "MISSING INFORMATION";
                alert.Content = "Please enter full information of book before upload!!!";
                alert.ShowDuration = 3000;
                RadDesktopAlertManager manager = new RadDesktopAlertManager();
                manager.ShowAlert(alert);
            }
            else
            {
                var product_BUS = new Product_BUS();
                var result = await product_BUS.AddProduct(_book);
                var alert = new RadDesktopAlert();
                if (result.Length != 0)
                {
                    alert.Header = "ADD NEW BOOK SUCCESSFULLy";
                    alert.Content = "Congratulation, your book was added!!!";
                    alert.ShowDuration = 3000;
                    this.DialogResult = true;
                    MainWindow.modelBinding.totalProduct++;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void RadRibbonButton_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Browse_Image_Btn(object sender, RoutedEventArgs e)
        {
            FileInfo _selectedImage = null;
            var screen = new OpenFileDialog();
            screen.Filter = "Files|*.jpg;*.png";
            if (screen.ShowDialog() == true)
            {
                _selectedImage = new FileInfo(screen.FileName);
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(screen.FileName, UriKind.Absolute);
                bitmap.EndInit();
                CoverImage.Source = bitmap;

                var converter = new Base64ToBitmapConverter();
                string base64String = (string)converter.ConvertBack(bitmap, null, null, null);
                
                _book.ImageBase64= base64String;
            }
        }
    }
}
