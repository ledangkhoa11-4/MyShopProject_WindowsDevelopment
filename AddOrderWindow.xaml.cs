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
using Telerik.Windows.Controls;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;

namespace MyShopProject
{
    public class AddOrderScreenModel
    {

        public ObservableCollection<Book> listAllBook { get; set; }
        public ObservableCollection<Coupon> listAllCoupon { get; set; }
        public Order newOrder { get; set; }

        public AddOrderScreenModel()
        {
            listAllBook = new ObservableCollection<Book>();
            listAllCoupon = MainWindow.modelBinding.listCoupon;
            newOrder = new Order();
        }
    }
    public partial class AddOrderWindow : Window
    {
        private Book_BUS _bookBus { get; set; }
        private Order_BUS _orderBus { get; set; }
        public AddOrderScreenModel modelBinding { get; set; }
        private List<bool> isStillLoading = new List<bool>();
        
        public AddOrderWindow()
        {
            InitializeComponent();
            _bookBus = new Book_BUS();
            _orderBus = new Order_BUS();


        }
        private async void createOrderLoaded(object sender, RoutedEventArgs e)
        {
            modelBinding = new AddOrderScreenModel();
            modelBinding.listAllBook = await _bookBus.getAllBriefBook();
            this.DataContext = modelBinding;


        }

        private async void addProductToCartEvent(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            var addedItems = e.AddedItems;
            var removedItems = e.RemovedItems;
            foreach (Book product in addedItems)
            {
                this.modelBinding.newOrder.DetailCart.Add(new DetailOrder
                {
                    Book = product,
                    QuantityBuy = 1,
                    Price = product.SellingPrice,
                    TotalPrice = product.SellingPrice
                });
                isLoadingIndicator.IsBusy = true;
                isStillLoading.Add(true);
                var base64Image = await _bookBus.getImageBook(product._id);
                isStillLoading.Remove(true);
                if(isStillLoading.Count > 0 )
                    isLoadingIndicator.IsBusy = true;
                else isLoadingIndicator.IsBusy = false; ;
                product.ImageBase64 = $"{base64Image}";


            }
            foreach (Book productRm in removedItems)
            {

                var orderItemRemove = this.modelBinding.newOrder.DetailCart.FirstOrDefault(x => x.Book == productRm);
                this.modelBinding.newOrder.DetailCart.Remove(orderItemRemove);
            }

        }

        private void addCouponToCartEvent(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            foreach (Coupon c in e.RemovedItems)
            {
                if (this.modelBinding.newOrder.Coupon == c)
                    this.modelBinding.newOrder.Coupon = null;
            }
            foreach (Coupon c in e.AddedItems)
            {
                this.modelBinding.newOrder.Coupon = c;
            }
        }

        private void AddNewCoupon(object sender, RoutedEventArgs e)
        {
            var newCouponScreen = new AddCouponWindow();
            if (newCouponScreen.ShowDialog() == true)
            {
                MainWindow.modelBinding.listCoupon.Add(newCouponScreen.newCoupon);
            }
            
        }

        private void removeItemCartClick(object sender, RoutedEventArgs e)
        {
            var buttonClicked = e.Source as Button;
            var items = cartCombobox.SelectedItems;
            foreach ( Book item in items )
            {
                if(item._id == buttonClicked.Tag.ToString())
                {
                    cartCombobox.SelectedItems.Remove(item);
                    return;
                }
                   
            }
            //this.modelBinding.newOrder.BookAndQuantity.Remove(modelBinding.newOrder.BookAndQuantity.FirstOrDefault(item => item.Book._id == buttonClicked.Tag.ToString()));
        }

        private async void CreateOrderClick(object sender, RoutedEventArgs e)
        {
            var alert = new RadDesktopAlert();

            if (modelBinding.newOrder.Customer.Length == 0 || modelBinding.newOrder.DetailCart.Count == 0)
            {
                alert.Header = "MISSING INFORMATION";
                alert.Content = "Please enter full information of coupon before upload!!!";
                alert.ShowDuration = 3000;
            }
            else
            {
                string result = await _orderBus.AddOrder(modelBinding.newOrder);
                result = result.Replace("\"", "");
                Debug.WriteLine(result);
                if(result.Length>0)
                {
                    modelBinding.newOrder._id = result;
                    MainWindow.modelBinding.totalOrder = await _orderBus.getCountOrder();
                    if (MainWindow.modelBinding.listOrder.Count < MainWindow.modelBinding.orderPerPage)
                    {
                        MainWindow.modelBinding.listOrder.Add(modelBinding.newOrder);
                    }
                    alert.Header = "CREATE NEW ORDER SUCCESSFULLY";
                    alert.Content = "Your new order was uploaded!!!";
                    alert.ShowDuration = 3000;
                    Close();
                }    
            }
            RadDesktopAlertManager manager = new RadDesktopAlertManager();
            manager.ShowAlert(alert);
        }


    }
}
