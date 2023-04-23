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
using System.ComponentModel;
using System.Windows.Media;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Convert = System.Convert;
using Sheet = DocumentFormat.OpenXml.Spreadsheet.Sheet;
using Telerik.Windows.Controls.ChartView;
using Telerik.Charting;
using Telerik.Windows.Controls.Legend;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;
using MyShopProject.Converters;

namespace MyShopProject
{

    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Category> listCat { get; set; }
        public ObservableCollection<Book> listBook { get; set; }
        public ObservableCollection<Book> listAllBriefBook { get; set; }
        public ObservableCollection<Coupon> listCoupon { get; set; }
        public ObservableCollection<Order> listOrder { get; set; }

        public int orderPerPage { get; set; } = 9;
        public int totalOrder { get; set; } = 0;
        public bool isOrderFilter { get; set; } = false;
        public string startDay { get; set; } = "";
        public string endDay { get; set; } = "";

        public int productPerPage { get; set; } = 6;
        public int totalProduct { get; set; } = 0;
        public int countStock { get; set; } = 0;
        public int countTitles { get; set; } = 0;
        public string CurrentMonth { get; set; }
        public int AmountOfOrderByMonth { get; set; }
        public int AmountOfOrderByWeek { get; set; }
        
        public ObservableCollection<Book> bestSaleBook { get; set;}
        public MainViewModel()
        {
            listCat = new ObservableCollection<Category>();
            listBook = new ObservableCollection<Book>();
            listOrder = new ObservableCollection<Order>();
            listCoupon = new ObservableCollection<Coupon>();
            bestSaleBook = new ObservableCollection<Book>();

            listAllBriefBook = new ObservableCollection<Book>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public partial class MainWindow : Window
    {
        public Category_BUS category_BUS { get; set; }
        public Coupon_BUS coupon_BUS { get; set; }
        public Product_BUS product_BUS { get; set; }
        public Order_BUS order_BUS { get; set; }
        public Book_BUS book_BUS { get; set; }
        public Report_BUS report_BUS { get; set; }
        public ImportData_BUS import_BUS { get; set; }
        public static MainViewModel modelBinding { get; set; } = new MainViewModel();
        public Account currentUser = null;
        private List<string> hexCodes = new List<string>{
                "#FFD700", // Gold
                "#DC143C", // Crimson
                "#6A0DAD", // Royal Purple
                "#228B22", // Forest Green
                "#0066CC"  // Sapphire Blue
            };
        private int reportMode = 0;
        private int maximumYAxis = 0;
        public MainWindow()
        {
            InitializeComponent();
            product_BUS = new Product_BUS();
            category_BUS = new Category_BUS();
            coupon_BUS = new Coupon_BUS();
            order_BUS = new Order_BUS();
            book_BUS = new Book_BUS();
            import_BUS = new ImportData_BUS();
            report_BUS = new Report_BUS();
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

            string tabItem = (((sender as RadTabControl).SelectedItem as RadTabItem).Header as TextBlock).Text;
            switch (tabItem)
            {
                case "Dasboard":
                    break;

                case "Category":
                    cateLoaded();
                    break;

                case "Products":
                    productLoaded();
                    break;
                case "Orders":
                    orderTabLoaded();
                    break;
                case "Dashboard":
                    dashboardTabLoaded();
                    break;
                case "Report":
                    reportTabLoaded();
                    break;
                default:
                    return;
            }
        }
        private void cateLoaded()
        {

        }
        private async void reportTabLoaded()
        {
            modelBinding.listAllBriefBook = await book_BUS.getAllBriefBook();
            chart.HorizontalAxis = new CategoricalAxis();
            chart.VerticalAxis = new LinearAxis() { Maximum = maximumYAxis, Minimum = -1 };
            chart.Series.Clear();
            legend.Items = new LegendItemCollection();
            

        }
        private SplineSeries createLine(List<StatisticsProductByTime> listRp, String color, String id)
        {
            SplineSeries line = new SplineSeries();
            line.Stroke = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(color));
            line.StrokeThickness = 2;
            line.Tag = id;
            foreach(StatisticsProductByTime data in listRp)
            {
                line.DataPoints.Add(new CategoricalDataPoint() { Category = DateFormat.ConvertDateFormat(data.category), Value = data.quantitySelling });
                if (data.quantitySelling >= maximumYAxis)
                {
                    maximumYAxis = data.quantitySelling + 1;
                }
            }
            return line;
           
        }
        private async void reportProductSelected(object sender, SelectionChangeEventArgs e)
        {
            var legendCollection = legend.Items;
            var addedItems = e.AddedItems;
            var removedItems = e.RemovedItems;
            if(productReportCombobox.SelectedItems.Count> 4 && addedItems.Count > 0) {
                productReportCombobox.CloseDropDown();
                MessageBox.Show("Select up to 4 books at a time", "Maximum book selected", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
                productReportCombobox.SelectedItems.Remove(addedItems[0]);
                return;
            }
            if(reportMode == 0)
            {
                productReportCombobox.CloseDropDown();
                MessageBox.Show("Please select mode statistics first!!", "No mode report selected", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);

                productReportCombobox.SelectedItems.Clear();
                
                return;
            }
            var lines = chart.Series;
            
            foreach (Book product in addedItems)
            {
                var currentIdx = legendCollection.Count;
                legendCollection.Add(new LegendItem { Title = Book.EllipsizeString(product.Name), 
                    MarkerFill = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(hexCodes[currentIdx]))});
                List<StatisticsProductByTime>data = new List<StatisticsProductByTime>();
                if(reportMode == 1)
                {
                    var startDay = statisticStartDay.SelectedDate.Value.ToString("yyyy-MM-dd");
                    var endDay = statisticEndDay.SelectedDate.Value.ToString("yyyy-MM-dd");
                    data = await report_BUS.statisticProductByDate(startDay, endDay, product._id);
                    var newline = createLine(data, hexCodes[currentIdx], product._id);
                    chart.VerticalAxis = new LinearAxis() { Maximum = maximumYAxis , Minimum = -1 };
                    lines.Add(newline);
                }
            }
            foreach (Book productRm in removedItems)
            {
                var item = legendCollection.FirstOrDefault(item => item.Title == Book.EllipsizeString(productRm.Name));
                
                if(item!= null)
                {
                    var color = item.MarkerFill;
                    legendCollection.Remove(item);
                    var lineDelete = lines.FirstOrDefault(line => line.Tag.ToString() == productRm._id);
                    lines.Remove(lineDelete);
                }
                
            }
        }

        private async void orderTabLoaded()
        {
            orderBusyIndicator.IsBusy = true;
            modelBinding.listOrder = await order_BUS.getAllOrder(modelBinding.orderPerPage, orderPager.PageIndex);
            orderBusyIndicator.IsBusy = false;
        }
        private async void dashboardTabLoaded()
        {
            DateTime dt = DateTime.Now;
            modelBinding.CurrentMonth = "In " + dt.ToString("MMMM") + ":";
            modelBinding.AmountOfOrderByMonth = await order_BUS.getCountByCurMonth();
            modelBinding.AmountOfOrderByWeek = await order_BUS.getCountByCurWeek();
        }
        private async void productLoaded()
        {
            modelBinding.listBook.Clear();
            productBusyIndicator.IsBusy = true;
            var listProduct = await product_BUS.getProductWithPagination(productPager.PageIndex, modelBinding.productPerPage); ;
            productBusyIndicator.IsBusy = false;
            modelBinding.listBook.AddRange(listProduct);
            modelBinding.totalProduct= await product_BUS.getSize();
            imageLoading.IsBusy = true;
            foreach (Book b in listProduct)
            {
                string imageBase64 = await book_BUS.getImageBook(b._id);
                b.ImageBase64 = imageBase64;
            }
            imageLoading.IsBusy = false;
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
            if (result == MessageBoxResult.Yes)
            {
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
                if (result.Length != 0)
                {
                    latestItem._id = await category_BUS.getNewestId();
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
                        latestItem._id = await category_BUS.getNewestId();
                        alert.Header = "Success";
                        alert.Content = "Category insert successfully!!!";
                        alert.ShowDuration = 3000;
                        RadDesktopAlertManager manager = new RadDesktopAlertManager();
                        manager.ShowAlert(alert);
                    }
                }
                else if (isExist)
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

                var tmp = new EditProductWindow(modelBinding.listCat, bookSelected);
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

                

                string messageBoxText = "This action cannot be undone. Are you sure to delete this book?";
                string caption = "Delete Confirmation";
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult resultDel;
                resultDel = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                var alertDel = new RadDesktopAlert();
                if (resultDel == MessageBoxResult.Yes)
                {
                    var bookSelected = bookCardView.SelectedItem as Book;
                    var product_BUS = new Product_BUS();
                    Debug.WriteLine(bookSelected.ToString());
                    var result = await product_BUS.DelProduct(bookSelected);
                    var alert = new RadDesktopAlert();
                    if (result.ToString().Length != 0)
                    {

                        alert.Header = "DELETE BOOK SUCCESSFULLy";
                        alert.Content = "Congratulation, your book was deleted!!!";

                        alert.ShowDuration = 3000;

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
                }
                    

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
                    if (bookSelected != null)
                    {
                        bookCardView.SelectedItem = 0;
                        bookCardView.SelectedItem = bookSelected;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                contextMenu.Visibility = Visibility.Collapsed;
            }
        }

        private void newProductBtnClick(object sender, RoutedEventArgs e)
        {
            var tmp = new AddProductWindow(modelBinding.listCat);
            if (tmp.ShowDialog()== true)
            {
                productLoaded();
                this.DataContext = modelBinding;
            }
            
        }

        private async void windowLoaded(object sender, RoutedEventArgs e)
        {
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
            modelBinding.totalOrder = await order_BUS.getCountOrder();
           
            //Tab nào thì code ở tabloadded nhe, đừng code ở đây

            this.DataContext = modelBinding;
            var stockInfo = await product_BUS.CountStock();
            modelBinding.countStock = stockInfo.Item1;
            modelBinding.countTitles = stockInfo.Item2;

        }

        private ObservableCollection<CheckBox> GetAllCheckBoxes()
        {
            ObservableCollection<CheckBox> checkBoxList = new ObservableCollection<CheckBox>();
            for (int i = 0; i < listCateFilter.ItemContainerGenerator.Items.Count; i++)
            {
                var item = listCateFilter.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                if (item != null)
                {
                    var checkBox = FindVisualChild<CheckBox>(item);
                    if (checkBox != null)
                    {
                        checkBoxList.Add(checkBox);
                        Debug.WriteLine(checkBox.Content);
                    }
                }
            }
            return checkBoxList;
        }


        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    var result = FindVisualChild<T>(child);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }

        private void newOrderBtnClick(object sender, RoutedEventArgs e)
        {
            var newOrderScreen = new AddOrderWindow();
            newOrderScreen.ShowDialog();
        }


        private void viewDetailOrderEvent(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = listOrderGridView.SelectedItem as Order;
            if (selectedItem != null)
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
            ObservableCollection<Order> listOrder;
            if (!modelBinding.isOrderFilter) listOrder = await order_BUS.getAllOrder(limit, skip);
            else listOrder = await order_BUS.GetOrderByDate(modelBinding.startDay,modelBinding.endDay,limit, skip);
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
            try
            {
                int pageIndex = e.NewPageIndex; //start at 0
                if (pageIndex < 0) return;
                modelBinding.listBook.Clear();
                ObservableCollection<Book> listProduct=null;
                if(isFilter()==true) {
                    var seletedRange = new List<int>
                    {
                        (int)PriceFilter.RangeStart,
                        (int)PriceFilter.RangeEnd
                    };
                    if (FilterByPrice.IsChecked == false)
                    {
                        seletedRange[0] = 0;
                        seletedRange[1] = 0;
                    }
                    productBusyIndicator.IsBusy = true;
                    listProduct = await book_BUS.getBookByCategoryAndPricePagination(selectedItems, seletedRange, pageIndex, modelBinding.productPerPage);
                    productBusyIndicator.IsBusy = false;
                }
                else if (SearchContent.Text.Length>0)
                {
                    var seletedRange = new List<int>
                    {
                        (int)PriceFilter.RangeStart,
                        (int)PriceFilter.RangeEnd
                    };
                    if (FilterByPrice.IsChecked == false)
                    {
                        seletedRange[0] = 0;
                        seletedRange[1] = 0;
                    }
                    productBusyIndicator.IsBusy = true;
                    listProduct = await book_BUS.findBookWithName(SearchContent.Text, seletedRange, pageIndex, modelBinding.productPerPage);
                    productBusyIndicator.IsBusy = false;
                }
                else
                {
                    productBusyIndicator.IsBusy = true;
                    listProduct = await product_BUS.getProductWithPagination(pageIndex, modelBinding.productPerPage);
                    productBusyIndicator.IsBusy = false;
                }
                

                modelBinding.listBook = listProduct;
                imageLoading.IsBusy = true;
                foreach (Book b in listProduct)
                {
                    string imageBase64 = await book_BUS.getImageBook(b._id);
                    b.ImageBase64 = imageBase64;
                }
                imageLoading.IsBusy = false;
            }
            catch (Exception ex) { }
        }
       

        private async void DeleteOrderClick(object sender, RoutedEventArgs e)
        {
            var buttonClicked = sender as RadRibbonButton;
            var orderDeltele = modelBinding.listOrder.FirstOrDefault(order => order._id == buttonClicked.Tag.ToString());

            string messageBoxText = "This action cannot be undone. Are you sure to delete this order?";
            string caption = "Delete Confirmation";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;
            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            var alert = new RadDesktopAlert();
            if (result == MessageBoxResult.Yes)
            {
                var rs = await order_BUS.deletetOrder(orderDeltele);
                if (rs.Length != 0)
                {
                    alert.Header = "DELETE ORDER SUCCESSFULLy";
                    alert.Content = "Congratulation, your order was deleted!!!";

                    alert.ShowDuration = 3000;
                    modelBinding.totalOrder--;
                    modelBinding.listOrder.Remove(orderDeltele);
                }
                else
                {
                    alert.Header = "ERROR";
                    alert.Content = "There was an error on update database, please try again!!!";
                    alert.ShowDuration = 3000;
                }
                RadDesktopAlertManager manager = new RadDesktopAlertManager();
                manager.ShowAlert(alert);
            }
        }
        List<String>  selectedItems = new List<String>();
        ObservableCollection<CheckBox> checkedCatListFilter = new ObservableCollection<CheckBox>();
        private void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            checkedCatListFilter = GetAllCheckBoxes();
            
        }
        
        private async void ApplyFilterBtn_Click(object sender, RoutedEventArgs e)
        {
            selectedItems.Clear();
            foreach (CheckBox checkbox in checkedCatListFilter)
            {
                if (checkbox.IsChecked == true)
                {
                    selectedItems.Add(checkbox.Content.ToString());
                }
            }
            if(selectedItems.Count > 0)
            {
                var seletedRange = new List<int>
            {
                (int)PriceFilter.RangeStart,
                (int)PriceFilter.RangeEnd
            };
                if (FilterByPrice.IsChecked == false)
                {
                    seletedRange[0] = 0;
                    seletedRange[1] = 0;
                }
                filterIndicator.IsBusy = true;
                modelBinding.totalProduct = await book_BUS.getSizeBookByCategoryAndPrice(selectedItems, seletedRange);
                modelBinding.listBook = await book_BUS.getBookByCategoryAndPricePagination(selectedItems, seletedRange, 0, modelBinding.productPerPage);
                filterIndicator.IsBusy = false;

                imageLoading.IsBusy = true;
                foreach (Book b in modelBinding.listBook)
                {
                    string imageBase64 = await book_BUS.getImageBook(b._id);
                    b.ImageBase64 = imageBase64;
                }

                imageLoading.IsBusy = false;
            }
            else
            {
                productLoaded();
            }
            


        }
        private Boolean isFilter()
        {
            foreach (CheckBox checkbox in checkedCatListFilter)
            {
                if (checkbox.IsChecked == true)
                {
                    return true;
                }
            }
            return false;
        }

        private async void UnApplyFilterBtn_Click(object sender, RoutedEventArgs e)
        {
            modelBinding.totalProduct = await product_BUS.getSize();
            productLoaded();
            foreach (CheckBox checkbox in checkedCatListFilter)
            {
                checkbox.IsChecked= false;
            }
            FilterDropdown.IsOpen = false;
            FilterByPrice.IsChecked= false;
        }

        private async void filterByDayButton(object sender, RoutedEventArgs e)
        {
            try {
                modelBinding.isOrderFilter= true;
                modelBinding.startDay = startDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd");
                modelBinding.endDay = endDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd");
                //orderBusyIndicator.IsBusy = true;
                modelBinding.listOrder = await order_BUS.GetOrderByDate(modelBinding.startDay, modelBinding.endDay,modelBinding.orderPerPage, 0);
                //modelBinding.totalOrder = modelBinding.listOrder.Count;
                modelBinding.totalOrder = await order_BUS.getCountFilter(modelBinding.startDay, modelBinding.endDay);
                orderPager.PageIndex = 0;
                //orderBusyIndicator.IsBusy = false;
                filterDropDownButton.IsOpen = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void removeFilterButton(object sender, RoutedEventArgs e)
        {
            startDatePicker.SelectedDate = null;
            endDatePicker.SelectedDate = null;
            modelBinding.isOrderFilter= false;
            ObservableCollection<Order> listOrder;
            listOrder = await order_BUS.getAllOrder(modelBinding.orderPerPage,0);
            modelBinding.listOrder.Clear();
            modelBinding.listOrder.AddRange(listOrder);
            modelBinding.totalOrder = await order_BUS.getCountOrder();
            orderPager.PageIndex = 0;
        }

        private async void selectFieldBestSaleDashboard(object sender, MouseButtonEventArgs e)
        {
            var filterBy =  (sender as ListBoxItem).Content as string;
            bestSellDropdown.Content = filterBy;
            bestSellDropdown.IsOpen = false;
            var bestSale = new ObservableCollection<Book>();
            if(filterBy == "By week")
                bestSale = await product_BUS.getBestSellingProducts("week");
            if(filterBy == "By month")  
                bestSale = await product_BUS.getBestSellingProducts("month");
            if (filterBy == "By year")
                bestSale = await product_BUS.getBestSellingProducts("year");
            modelBinding.bestSaleBook.Clear();
            modelBinding.bestSaleBook.AddRange(bestSale);
            var panel = bestSaleCarousel.FindCarouselPanel();
            panel.MoveBy(bestSale.Count);
            foreach (Book b in bestSale)
            {
                string imageBase64 = await book_BUS.getImageBook(b._id);
                b.ImageBase64 = imageBase64;
            }
        }
        private async void SeachBtn_Click(object sender, RoutedEventArgs e)
        {
            if(SearchContent.Text.Length > 0)
            {
                
                var seletedRange = new List<int>
                {
                    (int)PriceFilter.RangeStart,
                    (int)PriceFilter.RangeEnd
                };
                if (FilterByPrice.IsChecked == false)
                {
                    seletedRange[0] = 0;
                    seletedRange[1] = 0;
                }
                productBusyIndicator.IsBusy= true;
                modelBinding.totalProduct = await book_BUS.countBookWithName(SearchContent.Text, seletedRange);
                modelBinding.listBook = await book_BUS.findBookWithName(SearchContent.Text, seletedRange, 0, modelBinding.productPerPage);
                productBusyIndicator.IsBusy= false;

                imageLoading.IsBusy = true;
                foreach (Book b in modelBinding.listBook)
                {
                    string imageBase64 = await book_BUS.getImageBook(b._id);
                    b.ImageBase64 = imageBase64;
                }
                imageLoading.IsBusy = false;
            }
            else
            {
                productLoaded();
            }
        }
        FileInfo _selectedFile;
        private void ChooseFileBtn_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();
            screen.Filter = "Files|*.xlsx";

            if (screen.ShowDialog() == true)
            {
                _selectedFile = new FileInfo(screen.FileName);
                TextChooseFileBtn.Text = _selectedFile.Name;
                ImportDataBtn.IsEnabled = true;
                Uri uri = new Uri("Images/export.png", UriKind.Relative);
                IconChooseFileBtn.Source = new BitmapImage(uri);
            }
        }
        
        private  void ImportDataBtn_Click(object sender, RoutedEventArgs e)
        {
            var filename = _selectedFile.FullName;
            import_BUS.getProductFromExcelFile(filename);
            import_BUS.getCategoryFromExcelFile(filename);
        }

        private void statisticByDay(object sender, RoutedEventArgs e)
        {
            try
            {
                var startDay = statisticStartDay.SelectedDate.Value.ToString("yyyy-MM-dd");
                var endDay = statisticEndDay.SelectedDate.Value.ToString("yyyy-MM-dd");
                reportMode = 1;
                statisticsDropdown.IsOpen = false;
                statisticsDropdown.Content = $"{startDay} - {endDay}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}