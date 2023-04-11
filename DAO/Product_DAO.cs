﻿using Newtonsoft.Json;
using MyShopProject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;

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
        public async Task<List<Book>> getWithPagination(int pageIndex, int limit)
        {
            var json = await API.getMethod($"/product?brief=false&pgIdx={pageIndex}&limit={limit}");
            var productList = JsonConvert.DeserializeObject<List<Book>>(json);
            return productList;
        }
        public async Task<int> getSize()
        {
            var json = await API.getMethod($"/product/count");
            //Debug.WriteLine( json );
            var size = JsonConvert.DeserializeObject<int>(json);
            return size;
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
        public async Task<String> editProduct(string content, string id)
        {
            var json = await API.postMethod($"/product/edit?id={id}", content);
            return json;
        }
        public async Task<String> delProduct(string content, string id)
        {
            var json = await API.postMethod($"/product/delete?id={id}", content);
            return json;
        }
        public async Task<int> countStock()
        {
            var json = await API.getMethod("/product/stock");
            //var size = JsonConvert.DeserializeObject(json[]);
            MessageBox.Show(json);
            return 1;
        }
    }
}
