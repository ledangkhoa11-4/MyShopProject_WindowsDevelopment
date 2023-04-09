using MyShopProject.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyShopProject.DAO
{
    public class Category_DAO
    {
        public async Task<List<Category>> getAll()
        {
            var json = await API.getMethod("/category");
            var categoryList = JsonConvert.DeserializeObject<List<Category>>(json);
            return categoryList;
        }
        public async Task<bool> checkExist(string categoryId)
        {
            var json = await API.getMethod($"/category/id={categoryId}");
            var category = JsonConvert.DeserializeObject<Account>(json);
            if (category == null) return false;
            return true;
        }
    }
}
