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

namespace MyShopProject
{
    public class EditOrderScreenModel
    {
        public ObservableCollection<Book> listAllBook { get; set; }
        public ObservableCollection<Coupon> listAllCoupon { get; set; }
        public Order currentOrder { get; set; }
        public EditOrderScreenModel()
        {
            listAllBook = new ObservableCollection<Book>();
            listAllCoupon = MainWindow.modelBinding.listCoupon;
            this.currentOrder = new Order();
            
        }
    }
    public partial class EditOrderWindow : Window
    {
        public EditOrderScreenModel modelBinding { get; set; }      
        private Book_BUS _bookBus { get; set; }
        private Order_BUS _orderBus { get; set; }
        private List<bool> isStillLoading = new List<bool>();
        public Order oldOrder { get; set; }
        public EditOrderWindow(Order currentOrder)
        {
            InitializeComponent();
            modelBinding = new EditOrderScreenModel();
            _bookBus = new Book_BUS();
            _orderBus = new Order_BUS();
            oldOrder = currentOrder;
        }

        private async void SaveOrderClick(object sender, RoutedEventArgs e)
        {
            var alert = new RadDesktopAlert();

            if (modelBinding.currentOrder.Customer.Length == 0 || modelBinding.currentOrder.DetailCart.Count == 0)
            {
                alert.Header = "MISSING INFORMATION";
                alert.Content = "Please enter full information of coupon before upload!!!";
                alert.ShowDuration = 3000;
            }
            else
            {
                string result = await _orderBus.UpdateOrder(modelBinding.currentOrder);
                if (result.Length > 0)
                {
                    for(int i = 0; i < MainWindow.modelBinding.listOrder.Count; i++){
                        if (MainWindow.modelBinding.listOrder[i]._id == modelBinding.currentOrder._id)
                        {
                            MainWindow.modelBinding.listOrder[i] = modelBinding.currentOrder;
                            break;
                        }
                    }
                    alert.Header = "CREATE NEW ORDER SUCCESSFULLY";
                    alert.Content = "Your new order was updated!!!";
                    alert.ShowDuration = 3000;
                    Close();
                }
            }
            RadDesktopAlertManager manager = new RadDesktopAlertManager();
            manager.ShowAlert(alert);
        }

        private async void editOrderWindowLoaded(object sender, RoutedEventArgs e)
        {
            modelBinding.listAllBook = await _bookBus.getAllBriefBook();
            this.DataContext = modelBinding;

            modelBinding.currentOrder._id = oldOrder._id;
            modelBinding.currentOrder.Customer = oldOrder.Customer;
            modelBinding.currentOrder.PurchaseDate = oldOrder.PurchaseDate;

            foreach(var cart in oldOrder.DetailCart)
            {
                modelBinding.currentOrder.DetailCart.Add(cart);
                var book = modelBinding.listAllBook.FirstOrDefault(book => book._id == cart.Book._id);
                cartCombobox.SelectedItems.Add(book);
            }
            if (oldOrder.Coupon != null && oldOrder.Coupon._id != null)
            {
                couponAddedCbb.SelectedItem = oldOrder.Coupon;
            } 
            else
                modelBinding.currentOrder.Coupon = null;

            isLoadingIndicator.IsBusy = true;
            foreach (DetailOrder cart in modelBinding.currentOrder.DetailCart)
            {
                string bookid = cart.Book._id;
                string image = await _bookBus.getImageBook(bookid);
                cart.Book.ImageBase64 = image;
            }
            isLoadingIndicator.IsBusy = false;
            cartCombobox.SelectionChanged += addProductToCartEvent;

            //modelBinding.listAllBook = await _bookBus.getAllBriefBook();
            //this.DataContext = modelBinding;
            //var listCart = modelBinding.currentOrder.DetailCart.ToList();
            //foreach (DetailOrder cart in listCart)
            //{
            //    var book = modelBinding.listAllBook.FirstOrDefault(book => book._id == cart.Book._id);
            //    cartCombobox.SelectedItems.Add(book);
            //}
            //cartCombobox.SelectionChanged += addProductToCartEvent;
            //isLoadingIndicator.IsBusy = true;
            //foreach(DetailOrder cart in modelBinding.currentOrder.DetailCart)
            //{
            //    string bookid = cart.Book._id;
            //    string image = await _bookBus.getImageBook(bookid);
            //    cart.Book.ImageBase64= image;
            //}
            //isLoadingIndicator.IsBusy = false;
        }
        private async void addProductToCartEvent(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            var addedItems = e.AddedItems;
            var removedItems = e.RemovedItems;
            foreach (Book product in addedItems)
            {
                this.modelBinding.currentOrder.DetailCart.Add(new DetailOrder
                {
                    Book = product,
                    QuantityBuy = 1
                });
                isLoadingIndicator.IsBusy = true;
                isStillLoading.Add(true);
                var base64Image = await _bookBus.getImageBook(product._id);
                isStillLoading.Remove(true);
                if (isStillLoading.Count > 0)
                    isLoadingIndicator.IsBusy = true;
                else isLoadingIndicator.IsBusy = false; ;
                product.ImageBase64 = $"{base64Image}";


            }
            foreach (Book productRm in removedItems)
            {
                var orderItemRemove = this.modelBinding.currentOrder.DetailCart.FirstOrDefault(x => x.Book._id == productRm._id);
                this.modelBinding.currentOrder.DetailCart.Remove(orderItemRemove);
            }

        }

        private void addCouponToCartEvent(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            foreach (Coupon c in e.RemovedItems)
            {
                if (this.modelBinding.currentOrder.Coupon == c)
                    this.modelBinding.currentOrder.Coupon = null;
            }
            foreach (Coupon c in e.AddedItems)
            {
                this.modelBinding.currentOrder.Coupon = c;
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
            foreach (Book item in items)
            {
                if (item._id == buttonClicked.Tag.ToString())
                {
                    cartCombobox.SelectedItems.Remove(item);
                    return;
                }

            }
        }
    }
}
