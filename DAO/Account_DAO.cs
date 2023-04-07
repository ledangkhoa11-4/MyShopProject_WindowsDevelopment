using MyShopProject.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MyShopProject.DAO
{
    class Account_DAO
    {
       
        public async Task<Account> getAccount(string content)
        {
            var json = await API.getMethod($"/account?username={content}");
            var account = JsonConvert.DeserializeObject<Account>(json);
            return account;
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
