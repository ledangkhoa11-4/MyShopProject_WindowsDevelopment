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
            var json = await API.getMethod($"/category?id={categoryId}");
            var category = JsonConvert.DeserializeObject<Category>(json.Substring(1,json.Length-2));
            if (category == null) return false;
            return true;
        }
        public async Task<String> addCategory(string category)
        {
            var json = await API.postMethod("/category", category);
            return json;
        }

        public async Task<String> editCategory(string category,string id)
        {
            var json = await API.postMethod($"/category?id={id}", category);
            return json;
        }
        public async Task<String> deleteCategory(string category, string id)
        {
            var json = await API.postMethod($"/category/delete?id={id}", category);
            return json;
        }
    }
}
