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
            Account account = new Account() { Username = username, Password = passwordIn64 };
            var account_BUS = new Account_BUS();
            var alert = new RadDesktopAlert();
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
    }
}
