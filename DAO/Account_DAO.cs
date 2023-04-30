using MyShopProject.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MyShopProject.DAO
{
    class Account_DAO
    {
       
        public async Task<Account> getAccount(string username,string password,string salt)
        {
            var json = await API.getMethod($"/account?username={username}");
            var account = JsonConvert.DeserializeObject<Account>(json);
            if(account != null)
            {
                byte[] entropyInBytes1 = Convert.FromBase64String(salt);
                byte[] cypherTextInBytes1 = Convert.FromBase64String(password);
                byte[] passwordInBytes1 = ProtectedData.Unprotect(cypherTextInBytes1,
                    entropyInBytes1,
                    DataProtectionScope.CurrentUser
                );
                string pass1 = Encoding.UTF8.GetString(passwordInBytes1);

                byte[] entropyInBytes2 = Convert.FromBase64String(account.Salt);
                byte[] cypherTextInBytes2 = Convert.FromBase64String(account.Password);
                byte[] passwordInBytes2 = ProtectedData.Unprotect(cypherTextInBytes2,
                    entropyInBytes2,
                    DataProtectionScope.CurrentUser
                );
                string pass2 = Encoding.UTF8.GetString(passwordInBytes2);
                if (pass1 == pass2)
                    return account;
            }
            return null;
        }

        public async Task<bool> checkExist(string content)
        {
            var json = await API.getMethod($"/account?username={content}");
            var account = JsonConvert.DeserializeObject<Account>(json);
            if (account == null) return false;
            return true;
        }

        public async Task<String> addAccount(string account)
        {
            var json = await API.postMethod("/account", account);
            return json;
        }
    }
}
