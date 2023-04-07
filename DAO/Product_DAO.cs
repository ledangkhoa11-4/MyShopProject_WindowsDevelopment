using Newtonsoft.Json;
using MyShopProject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShopProject.DAO
{
    public class Product_DAO
    {
        public API api;
        public Product_DAO()
        {
            this.api = new API("http://127.0.0.1:5000");
        }
        public async Task<List<Book>> getAll()
        {
            var json = await api.getMethod("/product?brief=false");
            var productList = JsonConvert.DeserializeObject<List<Book>>(json);
            return productList;
        }
    }
}
