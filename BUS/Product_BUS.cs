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
        public async Task<String> AddProduct(Book newBook)
        {
            var jsonData = JsonConvert.SerializeObject(newBook);
            return await product_DAO.addNewProduct(jsonData);
        }
    }
}
