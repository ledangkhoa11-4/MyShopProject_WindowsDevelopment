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

namespace MyShopProject
{
    public class MainViewModel
    {
       public ObservableCollection<Category> listCat { get; set; } 
       public ObservableCollection<Book> listBook { get; set; } 
       public ObservableCollection<Order> listOrder { get; set; }
        public ObservableCollection<Coupon> listCoupon { get; set; }

        public MainViewModel()
        {
            listCat = new ObservableCollection<Category>();
            listBook = new ObservableCollection<Book>();
            listOrder = new ObservableCollection<Order>();
            listCoupon = new ObservableCollection<Coupon>();
        }
    }

    public partial class MainWindow : Window
    {
        public Category_BUS category_BUS { get; set; }
        public Coupon_BUS coupon_BUS { get; set; }
        public static MainViewModel modelBinding { get; set; }
        public Account currentUser = null;
        public MainWindow()
        {
            InitializeComponent();
            category_BUS = new Category_BUS();
            coupon_BUS = new Coupon_BUS();


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

                case "Item3":
                    break;

                default:
                    return;
            }
            

        }
        private void cateLoaded()
        {
           
        }
        private void categoryGenerated2(object sender, Telerik.Windows.Controls.Data.DataForm.AutoGeneratingFieldEventArgs e)
        {
            if(e.PropertyName == "_id")
            {
                e.DataField.IsEnabled = false;
                return;
            }
        }

        private void selectCateEvent(object sender, Telerik.Windows.Controls.GridView.GridViewSelectedCellsChangedEventArgs e)
        {
            var currentItem = listCategory.SelectedItem as Category;
            currentCat.CurrentItem= currentItem;
        }

        private void beforeDelCat(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string messageBoxText = "This action cannot be undone. Are you sure to delete this category?";
            string caption = "Delete Confirmation";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            if (result == MessageBoxResult.Yes)
                e.Cancel = false;
            else
                e.Cancel = true;
            
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

            }
            
        }

        private void prepareFilter(object sender, Telerik.Windows.Controls.Data.CardView.CardViewFilteringEventArgs e)
        {
            var a = e.Added;
            foreach(var item in a)
            {
                Debug.WriteLine(item.ToString());
                
            }
        }

        private void editBookEvent(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var bookSelected = bookCardView.SelectedItem as Book;
                Debug.WriteLine(bookSelected.Name);

              
            }
            catch(Exception ex)
            {
                //ignore
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

            Debug.WriteLine(contextMenu.Visibility);
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
                    bookCardView.SelectedItem = bookSelected;
                    return;
                   
                }
            }catch(Exception ex)
            {
               contextMenu.Visibility= Visibility.Collapsed;
               
            }
        }

        private void newProductBtnClick(object sender, RoutedEventArgs e)
        {
            var tmp = new AddProductWindow();
            tmp.ShowDialog();
        }

        private async void windowLoaded(object sender, RoutedEventArgs e)
        {
            var testConn =  await API.testConnection();
            if(testConn.Item1 == false)
            {
                MessageBox.Show(testConn.Item2 + "Application will close", "Error connect web service");
                System.Windows.Application.Current.Shutdown();
            }
            var login = new LoginWindow();
            if(login.ShowDialog() == true) {
                currentUser = login.currentAccount;
                modelBinding = new MainViewModel();
                modelBinding.listCat = await category_BUS.getAllCategory();
                modelBinding.listCoupon = await coupon_BUS.getAllCoupon();
                this.DataContext = modelBinding;
            }
            
        }

        private void newOrderBtnClick(object sender, RoutedEventArgs e)
        {
            var newOrderScreen = new AddOrderWindow();
            newOrderScreen.ShowDialog();
        }
    }
}
