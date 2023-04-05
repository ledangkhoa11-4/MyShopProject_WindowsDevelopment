using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    public class AddOrderScreenModel{
        public ObservableCollection<Book> listAllBook { get; set; }
        public Order newOrder { get; set; }
        public Book bookTest { get; set; }
        public AddOrderScreenModel()
        {
            listAllBook = MainWindow.modelBinding.listBook;
            newOrder = new Order();
            bookTest = listAllBook[0];
        }
     }
    public partial class AddOrderWindow : Window
    {
        public AddOrderScreenModel modelBinding { get; set; }
        public AddOrderWindow()
        {
            InitializeComponent();


        }
        private void createOrderLoaded(object sender, RoutedEventArgs e)
        {
            modelBinding = new AddOrderScreenModel();

            this.DataContext = modelBinding;


        }

        private void addProductToCartEvent(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            var addedItems = e.AddedItems;
            var removedItems = e.RemovedItems;
            
            foreach (Book product in addedItems)
            {
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
    }
}
