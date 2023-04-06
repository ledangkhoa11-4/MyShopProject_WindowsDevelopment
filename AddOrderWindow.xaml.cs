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
        public AddOrderScreenModel modelBinding { get; set; }
        public AddOrderWindow()
        {
            InitializeComponent();
            _bookBus = new Book_BUS();


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
                var base64Image = await _bookBus.getImageBook(product._id);

                product.ImageBase64 = $"{base64Image}";
                this.modelBinding.newOrder.BookAndQuantity.Add(new DetailOrder
                {
                    Book = product,
                    QuantityBuy = 1
                });

            }
            foreach (Book productRm in removedItems)
            {

                var orderItemRemove = this.modelBinding.newOrder.BookAndQuantity.FirstOrDefault(x => x.Book == productRm);
                this.modelBinding.newOrder.BookAndQuantity.Remove(orderItemRemove);
            }

        }

        private void addCouponToCartEvent(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            foreach (Coupon c in e.AddedItems)
            {
                this.modelBinding.newOrder.Coupon = c;
            }
            foreach (Coupon c in e.RemovedItems)
            {
                this.modelBinding.newOrder.Coupon = null;
            }
        }

        private void AddNewCoupon(object sender, RoutedEventArgs e)
        {
            var newCouponScreen = new AddCouponWindow();
            newCouponScreen.ShowDialog();
        }
    }
}
