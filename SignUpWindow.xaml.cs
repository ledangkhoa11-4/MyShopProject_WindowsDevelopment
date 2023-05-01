using MyShopProject.BUS;
using MyShopProject.DTO;
using System;
using System.Collections.Generic;
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

namespace MyShopProject
{
    /// <summary>
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        public SignUpWindow()
        {
            InitializeComponent();
        }

        private async void signupButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordBox.Password;
            var passwordInBytes = Encoding.UTF8.GetBytes(password);
            var alert = new RadDesktopAlert();
            Account account;
            using (AesManaged aes = new AesManaged())
            {
                string passwordHash = Convert.ToBase64String(Account.Encrypt(passwordInBytes, aes.Key, aes.IV));
                account = new Account() { Username = username, Password = passwordHash, Key = Convert.ToBase64String(aes.Key), IV = Convert.ToBase64String(aes.IV) };
            }
            if (account != null)
            {
                var account_BUS = new Account_BUS();
                bool check = await account_BUS.checkExists(account);
                if (check)
                {
                    alert.Header = "ERROR";
                    alert.Content = "Account existed, please try again!!!";
                    alert.ShowDuration = 3000;
                    RadDesktopAlertManager manager = new RadDesktopAlertManager();
                    manager.ShowAlert(alert);
                }
                else
                {
                    var result = await account_BUS.addAccount(account);

                    if (result.Length != 0)
                    {
                        alert.Header = "Success";
                        alert.Content = "Sign up successfully!!!";
                        alert.ShowDuration = 3000;
                        RadDesktopAlertManager manager = new RadDesktopAlertManager();
                        manager.ShowAlert(alert);
                        this.DialogResult = true;
                        this.Close();
                    }
                    else
                    {
                        alert.Header = "ERROR";
                        alert.Content = "There was an error, please try again!!!";
                        alert.ShowDuration = 3000;
                        RadDesktopAlertManager manager = new RadDesktopAlertManager();
                        manager.ShowAlert(alert);
                    }
                }
            }
            else
            {
                alert.Header = "ERROR";
                alert.Content = "There was an error, please try again!!!";
                alert.ShowDuration = 3000;
                RadDesktopAlertManager manager = new RadDesktopAlertManager();
                manager.ShowAlert(alert);
            }
        }
    }
}
