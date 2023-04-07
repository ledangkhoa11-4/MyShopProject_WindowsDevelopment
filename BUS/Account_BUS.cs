using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShopProject.DAO;
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
        public async Task<Account?> GetAccount(string username)
        {
            Account? res = await account_DAO.getAccount(username);
            return res;
        }
    }
}
