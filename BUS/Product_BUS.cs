using MyShopProject.DAO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MyShopProject.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Telerik.Windows.Controls.GridView;

namespace MyShopProject.BUS
{
    public class Product_BUS
    {
        Product_DAO product_DAO;
        public Product_BUS()
        {
            product_DAO = new Product_DAO();
        }
        public async Task<ObservableCollection<Book>> getAllProduct()
        {
            List<Book> res = await product_DAO.getAll();
            return new ObservableCollection<Book>(res);
        }
        public async Task<Book> getProduct(string id, bool isBrief)
        {
            var res = await product_DAO.getById(id, isBrief);
            return res;
        }
        public async Task<ObservableCollection<Book>> getProductWithPagination(int pageIndex, int limit)
        {
            List<Book> res = await product_DAO.getWithPagination(pageIndex, limit);
            return new ObservableCollection<Book>(res);
        }
        public async Task<int> getSize()
        {
            int res = await product_DAO.getSize();
            return res;
        }
        public async Task<String> AddProduct(Book newBook)
        {
            var jsonData = JsonConvert.SerializeObject(newBook);
            return await product_DAO.addNewProduct(jsonData);
        }
        public async Task<String> EditProduct(Book book)
        {
            var jsonData = JsonConvert.SerializeObject(book);
            return await product_DAO.editProduct(jsonData, book._id);
        }
        public async Task<String> DelProduct(Book book)
        {
            var jsonData = JsonConvert.SerializeObject(book);
            return await product_DAO.delProduct(jsonData, book._id);
        }

        public async Task<Tuple<int,int>> CountStock()
        {
            var result = await product_DAO.countStock();
            JObject jsonObject = JObject.Parse(result);
            jsonObject.Remove("_id");
            var stock = int.Parse((string)jsonObject["totalQty"]);
            var tls =  int.Parse((string)jsonObject["totalTls"]);
            return Tuple.Create(stock, tls);
        }
        public async Task<ObservableCollection<Book>> getBestSellingProducts(string filterby)
        {
            var listBook = await product_DAO.getBestSellingProducts(filterby);
            return new ObservableCollection<Book>(listBook);
        }
        public async Task<ObservableCollection<Book>> getLowStockProducts()
        {
            var listBook = await product_DAO.getLowStockProducts();
            return new ObservableCollection<Book>(listBook);
        }
    }
}
