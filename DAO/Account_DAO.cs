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
       
        public async Task<Account> getAccount(string username,string password,string key,string iv)
        {
            var json = await API.getMethod($"/account?username={username}");
            var account = JsonConvert.DeserializeObject<Account>(json);
            if(account != null)
            {
                byte[] keyInBytes1 = Convert.FromBase64String(key);
                byte[] ivInBytes1 = Convert.FromBase64String(iv);
                byte[] passwordInBytes1 = Convert.FromBase64String(password);
                byte[] passwordHash1 = Account.Decrypt(passwordInBytes1, keyInBytes1, ivInBytes1);
                string pass1 = Encoding.UTF8.GetString(passwordHash1);

                byte[] keyInBytes2 = Convert.FromBase64String(account.Key);
                byte[] ivInBytes2 = Convert.FromBase64String(account.IV);
                byte[] passwordInBytes2 = Convert.FromBase64String(account.Password);
                byte[] passwordHash2 = Account.Decrypt(passwordInBytes2, keyInBytes2, ivInBytes2);
                string pass2 = Encoding.UTF8.GetString(passwordHash2);

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
