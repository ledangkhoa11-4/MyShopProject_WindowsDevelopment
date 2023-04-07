﻿using Newtonsoft.Json;
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
        public async Task<List<Book>> getAll()
        {
            var json = await API.getMethod("/product?brief=false");
            var productList = JsonConvert.DeserializeObject<List<Book>>(json);
            return productList;
        }
        public async Task<Book> getById(string id)
        {
            var json = await API.getMethod($"/product?id={id}");
            var res = JsonConvert.DeserializeObject<Book>(json);
            return res;
        }
        public async Task<String> addNewProduct(string content)
        {
            var json = await API.postMethod("/product", content);
            return json;
        }
    }
}
