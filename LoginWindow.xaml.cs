using MyShopProject.BUS;
using MyShopProject.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
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
using Convert = System.Convert;

namespace MyShopProject
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public Account currentAccount;
        public LoginWindow()
        {
            InitializeComponent();
            string username = ConfigurationManager.AppSettings["Username"]!;
            string passwordIn64 = ConfigurationManager.AppSettings["Password"]!;
            string entropyIn64 = ConfigurationManager.AppSettings["Entropy"]!;
            if (passwordIn64.Length != 0)
            {
                byte[] entropyInBytes = Convert.FromBase64String(entropyIn64);
                byte[] cypherTextInBytes = Convert.FromBase64String(passwordIn64);

                byte[] passwordInBytes = ProtectedData.Unprotect(cypherTextInBytes,
                    entropyInBytes,
                    DataProtectionScope.CurrentUser
                );

                string password = Encoding.UTF8.GetString(passwordInBytes);

                usernameTextBox.Text = username;
                passwordBox.Password = password;
                rememberCheckBox.SetCurrentValue(CheckBox.IsCheckedProperty, true);

            }
        }

        private async void connectButton_Click(object sender, RoutedEventArgs e)
        {
            loginBusyIndicator.IsBusy = true;
            string username = usernameTextBox.Text;
            string password = passwordBox.Password;
            var passwordInBytes = Encoding.UTF8.GetBytes(password);
            var entropy = new byte[20];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(entropy);
            }

            var cypherText = ProtectedData.Protect(
                passwordInBytes,
                entropy,
                DataProtectionScope.CurrentUser
            );

            var passwordIn64 = Convert.ToBase64String(cypherText);
            var entropyIn64 = Convert.ToBase64String(entropy);
            Account account = new Account() { Username= username, Password = passwordIn64, Salt = entropyIn64 };
            var account_BUS = new Account_BUS();
            var result = await account_BUS.getAccount(account);
            var alert = new RadDesktopAlert();
            if(rememberCheckBox.IsChecked== true)
            {
                var config = ConfigurationManager.OpenExeConfiguration(
                        ConfigurationUserLevel.None);
                config.AppSettings.Settings["Username"].Value = username;
                config.AppSettings.Settings["Password"].Value = passwordIn64;
                config.AppSettings.Settings["Entropy"].Value = entropyIn64;

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            result = new Account();
            if (result != null)
            {
                currentAccount = result;
                //this.DialogResult = true;
                var category_BUS = new Category_BUS();
                var product_BUS = new Product_BUS();
                var coupon_BUS = new Coupon_BUS();
                var order_BUS = new Order_BUS();
                //MainWindow.modelBinding.listCat = await category_BUS.getAllCategory();
                MainWindow.modelBinding.totalProduct = await product_BUS.getSize();
                MainWindow.modelBinding.listCoupon = await coupon_BUS.getAllCoupon();
                MainWindow.modelBinding.totalOrder = await order_BUS.getCountOrder();
                int lastTab = 0;
                int.TryParse(ConfigurationManager.AppSettings["LastTab"],out lastTab);
                MainWindow.modelBinding.lastTab = lastTab;
                var mainWindow = new MainWindow();
                mainWindow.currentUser= currentAccount;
                mainWindow.DataContext = MainWindow.modelBinding;
                mainWindow.Show();
                loginBusyIndicator.IsBusy = false;
                this.Close();
            }
            else
            {
                alert.Header = "ERROR";
                alert.Content = "Unvalid account, please check again!!!";
                alert.ShowDuration = 3000;
                RadDesktopAlertManager manager = new RadDesktopAlertManager();
                manager.ShowAlert(alert);
                loginBusyIndicator.IsBusy = false;
            }
            loginBusyIndicator.IsBusy = false;
        }

        private void signupButton_Click(object sender, RoutedEventArgs e)
        {
            var signup = new SignUpWindow();
            if(signup.ShowDialog() == true)
            {

            }
        }
    }
}
