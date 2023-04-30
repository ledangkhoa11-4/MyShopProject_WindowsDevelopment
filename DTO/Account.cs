using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyShopProject.DTO
{
    public class Account : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string username;
        private string password;
        private string salt;
        public string Username {
            get { return string.Join("", username.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)); }
            set { username = value; } 
        }    
        public string Password {
            get { return string.Join("", password.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)); }
            set { password = value; }
        }
        public string Salt
        {
            get { return string.Join("", salt.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)); }
            set { salt = value; }
        }
    }

}
