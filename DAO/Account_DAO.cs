using MyShopProject.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyShopProject.DAO
{
    class Account_DAO
    {
       
        public async Task<Account?> getAccount(string username)
        {
            var json = await API.getMethod($"/account/{username}");
            var account = JsonConvert.DeserializeObject<Account>(json);
            return account;
        }
        public async Task<Boolean> addAccount(Account account)
        {// /account đâu?
            string http = "http://127.0.0.1:5000";
            var content = new StringContent(JsonConvert.SerializeObject(account),Encoding.UTF8, "application/json");
            var client = new HttpClient();
            var response = await client.PostAsync(http, content);
            if (response != null) return true;
            return false;
        }
    }
}
