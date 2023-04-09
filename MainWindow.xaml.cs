using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Telerik.Windows.Controls;
using Category = MyShopProject.DTO.Category;
using Book = MyShopProject.DTO.Book;
using MyShopProject.BUS;
using MyShopProject.DTO;
using MyShopProject.DAO;
using Telerik.Windows.Persistence.Core;
using System.ComponentModel;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;

namespace MyShopProject
{
 
    public class MainViewModel:INotifyPropertyChanged
    {
       public ObservableCollection<Category> listCat { get; set; } 
       public ObservableCollection<Book> listBook { get; set; } 
       
       public ObservableCollection<Coupon> listCoupon { get; set; }

        public ObservableCollection<Order> listOrder { get; set; }

        public  int orderPerPage { get; set; } = 9;
        public  int totalOrder { get; set; } = 0;

        public int productPerPage { get; set; } = 6;
        public int totalProduct { get; set; } = 0;

        public MainViewModel()
        {
            listCat = new ObservableCollection<Category>();
            listBook = new ObservableCollection<Book>();
            listOrder = new ObservableCollection<Order>();
            listCoupon = new ObservableCollection<Coupon>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public partial class MainWindow : Window
    {
        public Category_BUS category_BUS { get; set; }
        public Coupon_BUS coupon_BUS { get; set; }
        public Product_BUS product_BUS { get; set; }
        public Order_BUS order_BUS { get; set; }
        public static MainViewModel modelBinding { get; set; }
        public Account currentUser = null;
        public MainWindow()
        {
            InitializeComponent();
            product_BUS = new Product_BUS();
            category_BUS = new Category_BUS();
            coupon_BUS = new Coupon_BUS();
            order_BUS = new Order_BUS();

            modelBinding = new MainViewModel();
        }

        private void chooseImageClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            dialog.Filter = "Image files (*.jpg, *.png)|*.jpg;*.png";
            if (dialog.ShowDialog() == true)
            {
                string imagePath = dialog.FileName;
                BitmapImage bitmap = new BitmapImage(new Uri(imagePath));

                MemoryStream ms = new MemoryStream();
                JpegBitmapEncoder encoder = new JpegBitmapEncoder(); // or PngBitmapEncoder
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(ms);
                byte[] imageData = ms.ToArray();
                string base64String = System.Convert.ToBase64String(imageData);


                Debug.WriteLine(base64String);
            }
        }


        private void tabChanged(object sender, Telerik.Windows.Controls.RadSelectionChangedEventArgs e)
        {

            string tabItem = ((sender as RadTabControl).SelectedItem as RadTabItem).Header as string;

            switch (tabItem)
            {
                case "Dasboard":
                    break;

                case "Category":
                    cateLoaded();
                    break;

                case "Product":
                    productLoaded();
                    break;

                default:
                    return;
            }


        }
        private void cateLoaded()
        {

        }
        private async void productLoaded()
        {
            modelBinding.listBook.Clear();

  
            var listProduct = await product_BUS.getProductWithPagination(productPager.PageIndex, modelBinding.productPerPage);;

            modelBinding.listBook.AddRange(listProduct);
        }
        private void categoryGenerated2(object sender, Telerik.Windows.Controls.Data.DataForm.AutoGeneratingFieldEventArgs e)
        {
            if (e.PropertyName == "_id")
            {
                e.DataField.IsEnabled = false;
   
            }
            if (e.PropertyName == "Description")
            {
                e.DataField.MaxWidth = 1300;
                
                
            }
        }

        private void selectCateEvent(object sender, Telerik.Windows.Controls.GridView.GridViewSelectedCellsChangedEventArgs e)
        {
            var currentItem = listCategory.SelectedItem as Category;
            currentCat.CurrentItem = currentItem;
        }

        private async void beforeDelCat(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string messageBoxText = "This action cannot be undone. Are you sure to delete this category?";
            string caption = "Delete Confirmation";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;
            var alert = new RadDesktopAlert();

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            if (result == MessageBoxResult.Yes) {
                e.Cancel = false;
                var currentItem = listCategory.SelectedItem as Category;
                var ans = await category_BUS.deleteCate(currentItem);
                if (ans.Length != 0)
                {
                    alert.Header = "Success";
                    alert.Content = "Category delete successfully!!!";
                    alert.ShowDuration = 3000;
                    RadDesktopAlertManager manager = new RadDesktopAlertManager();
                    manager.ShowAlert(alert);
                }
                else
                {
                    alert.Header = "Error";
                    alert.Content = "Error occurred, please try again!!!";
                    alert.ShowDuration = 3000;
                    RadDesktopAlertManager manager = new RadDesktopAlertManager();
                    manager.ShowAlert(alert);
                }
            }
            else
                e.Cancel = true;

        }

        private async void afterEditCat(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        {
            var currentItem = listCategory.SelectedItem as Category;
            var alert = new RadDesktopAlert();
            if (currentItem == null)
            {
                var latestItem = listCategory.Items[listCategory.Items.Count - 1] as Category;
                var result = await category_BUS.addCategory(latestItem);
                if(result.Length != 0)
                {
                    alert.Header = "Success";
                    alert.Content = "Category insert successfully!!!";
                    alert.ShowDuration = 3000;
                    RadDesktopAlertManager manager = new RadDesktopAlertManager();
                    manager.ShowAlert(alert);
                }
            }
            else
            {
                var latestItem = listCategory.Items[listCategory.Items.Count - 1] as Category;
                bool isExist = await category_BUS.checkExist(latestItem);
                if (!isExist && latestItem.Name != "")
                {
                    var result = await category_BUS.addCategory(latestItem);
                    if (result.Length != 0)
                    {
                        alert.Header = "Success";
                        alert.Content = "Category insert successfully!!!";
                        alert.ShowDuration = 3000;
                        RadDesktopAlertManager manager = new RadDesktopAlertManager();
                        manager.ShowAlert(alert);
                    }
                }
                else if(isExist)
                {
                    var result = await category_BUS.editCategory(currentItem);
                    if (result.Length != 0)
                    {
                        alert.Header = "Success";
                        alert.Content = "Category update successfully!!!";
                        alert.ShowDuration = 3000;
                        RadDesktopAlertManager manager = new RadDesktopAlertManager();
                        manager.ShowAlert(alert);
                    }
                }
            }
        }

        private void CateTableLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            try
            {
                listCategory.Columns[2].TextWrapping = TextWrapping.Wrap;
                listCategory.Columns[1].TextWrapping = TextWrapping.Wrap;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        private void prepareFilter(object sender, Telerik.Windows.Controls.Data.CardView.CardViewFilteringEventArgs e)
        {
            var a = e.Added;
            foreach (var item in a)
            {
                Debug.WriteLine(item.ToString());

            }
        }

        private void editBookEvent(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var bookSelected = bookCardView.SelectedItem as Book;
                Debug.WriteLine(bookSelected._id);

                var tmp = new EditProductWindow(modelBinding.listCat, bookSelected);
                tmp.ShowDialog();
                //if (tmp.DialogResult == true)
                //{
                //    productLoaded();
                //    this.DataContext = modelBinding;
                //}
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
        private void editBookClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            try
            {
                var selectedBook = bookCardView.SelectedItem as Book;
                Debug.WriteLine(selectedBook._id);

                var tmp = new EditProductWindow(modelBinding.listCat, selectedBook);
                tmp.ShowDialog();
                if (tmp.DialogResult == true)
                {
                    productLoaded();
                    this.DataContext = modelBinding;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async void deleteBookClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            try
            {
               
                var bookSelected = bookCardView.SelectedItem as Book;
                var product_BUS = new Product_BUS();
                Debug.WriteLine(bookSelected.ToString());
                var result = await product_BUS.DelProduct(bookSelected);
                var alert = new RadDesktopAlert();
                if (result.Length != 0)
                {
                    alert.Header = "DELETE BOOK SUCCESSFULLy";
                    alert.Content = "Congratulation, your book was deleted!!!";
                   
                    alert.ShowDuration = 3000;
                    this.DialogResult = true;
                    modelBinding.totalProduct = modelBinding.totalProduct - 1;
                }
                else
                {
                    alert.Header = "ERROR";
                    alert.Content = "There was an error on update database, please try again!!!";
                    alert.ShowDuration = 3000;
                }
                RadDesktopAlertManager manager = new RadDesktopAlertManager();
                manager.ShowAlert(alert);
                productLoaded();
                this.DataContext = modelBinding;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private void RadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            try
            {
                var bookSelected = bookCardView.SelectedItem as Book;
                Debug.WriteLine(bookSelected.Name);

            }
            catch (Exception ex)
            {
                //ignore
            }
        }


        private void rightButtonDownRadCard(object sender, MouseButtonEventArgs e)
        {
            try
            {
                contextMenu.Visibility = Visibility.Visible;
                var clickElement = e.OriginalSource as FrameworkElement;
                var holderItem = clickElement.ParentOfType<RadCardViewItem>();
                var allTextBlockChild = holderItem.ChildrenOfType<TextBlock>();
                foreach (var tb in allTextBlockChild)
                {
                    var ID = tb.Text;
                    
                    var bookSelected = modelBinding.listBook.FirstOrDefault(book => book._id == ID);
                    if(bookSelected!= null)
                    {
                        bookCardView.SelectedItem = 0;
                        bookCardView.SelectedItem = bookSelected;
                        return;
                    }
                } 
            }catch(Exception ex)
            {
               contextMenu.Visibility= Visibility.Collapsed;               
            }
        }

        private void newProductBtnClick(object sender, RoutedEventArgs e)
        {
            var tmp = new AddProductWindow(modelBinding.listCat);
            tmp.ShowDialog();
        }

        private async void windowLoaded(object sender, RoutedEventArgs e)
        {
            //hiện loading lúc đang query db cho đỡ trống trãi
            orderBusyIndicator.IsBusy = true;

            var testConn = await API.testConnection();
            if (testConn.Item1 == false)
            {
                MessageBox.Show(testConn.Item2 + "Application will close", "Error connect web service");
                System.Windows.Application.Current.Shutdown();
            }
            var login = new LoginWindow();
            if (login.ShowDialog() == true)
            {
                currentUser = login.currentAccount;
                
            }
            
            modelBinding.listCat = await category_BUS.getAllCategory();

            modelBinding.totalProduct = await product_BUS.getSize();
            
         
            modelBinding.listCoupon = await coupon_BUS.getAllCoupon();
            modelBinding.listOrder =  await order_BUS.getAllOrder(modelBinding.orderPerPage,0);

            modelBinding.totalOrder = await order_BUS.getCountOrder();
            this.DataContext = modelBinding;
           
 

            //tắt loading
            orderBusyIndicator.IsBusy = false;
        }
        

        private void newOrderBtnClick(object sender, RoutedEventArgs e)
        {
            var newOrderScreen = new AddOrderWindow();
            newOrderScreen.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void viewDetailOrderEvent(object sender, MouseButtonEventArgs e)
        {
           
            var selectedItem = listOrderGridView.SelectedItem as Order;
            if(selectedItem != null)
            {
                var detailOrder = new OrderDetailWindow(selectedItem);
                detailOrder.Show();
            }
           
            
        }

        private async void changeOrderPage(object sender, PageIndexChangedEventArgs e)
        {
            int pageIndex = e.NewPageIndex; //start at 0
            int limit = modelBinding.orderPerPage;
            int skip = pageIndex * limit;
            orderBusyIndicator.IsBusy = true;
            var listOrder = await order_BUS.getAllOrder(limit, skip);
            modelBinding.listOrder.Clear();
            modelBinding.listOrder.AddRange(listOrder);

            foreach (Order order in modelBinding.listOrder)
            {
                if (order.Coupon != null && order.Coupon._id != null)
                    order.Coupon = modelBinding.listCoupon.FirstOrDefault(cp => cp._id == order.Coupon._id);
            }

            orderBusyIndicator.IsBusy = false;
        }

        private void EditOrderClick(object sender, RoutedEventArgs e)
        {
            var buttonClicked = sender as RadRibbonButton;
            var orderEditing = modelBinding.listOrder.FirstOrDefault(order => order._id == buttonClicked.Tag.ToString());

            var cloneNewOrder = (Order)orderEditing.Clone();
            var editScreen = new EditOrderWindow(cloneNewOrder);
            editScreen.ShowDialog();
        }

        private async void changeProductPage(object sender, PageIndexChangedEventArgs e)
        {
            int pageIndex = e.NewPageIndex; //start at 0
            modelBinding.listBook.Clear();

            productBusyIndicator.IsBusy = true;
            var listProduct = await product_BUS.getProductWithPagination(pageIndex, modelBinding.productPerPage);
            productBusyIndicator.IsBusy = false;
            
            modelBinding.listBook.AddRange(listProduct);
            
        }
    }
}
