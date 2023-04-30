using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShopProject.DAO;
using MyShopProject.DTO;
using Newtonsoft.Json;
using Account = MyShopProject.DTO.Account;
namespace MyShopProject.BUS
{
    class Account_BUS
    {
        Account_DAO account_DAO;
        public Account_BUS()
        {
            account_DAO = new Account_DAO();
        }
        public async Task<Account> getAccount(Account account)
        {
            Account res = await account_DAO.getAccount(account.Username,account.Password,account.Salt);
            return res;
        }
        public async Task<bool> checkExists(Account account)
        {
            return await account_DAO.checkExist(account.Username);
        }
        public async Task<String> addAccount(Account account)
        {
            var jsonData = JsonConvert.SerializeObject(account);
            return await account_DAO.addAccount(jsonData);
        }
    }
}
