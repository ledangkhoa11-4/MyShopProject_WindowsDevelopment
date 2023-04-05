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
        private API api;
        public Category_DAO()
        {
            this.api = new API("http://127.0.0.1:5000");
        }
        public async Task<List<Category>> getAll()
        {
            var json = await api.getMethod("/category");
            var categoryList = JsonConvert.DeserializeObject<List<Category>>(json);
            return categoryList;
        }
    }
}
