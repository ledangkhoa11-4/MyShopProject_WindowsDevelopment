using MyShopProject.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShopProject.DAO
{
    public class Book_DAO
    {
        public async Task<List<Book>> getAll(bool isBrief = false)
        { 
            var query = isBrief == true ? "?brief=true" : "";
            var json = await API.getMethod($"/product{query}");
            var bookList = JsonConvert.DeserializeObject<List<Book>>(json);
            return bookList;
        }
        public async Task<Book> get(string _id, bool isBrief = false)
        {
            var query = isBrief == true ? $"/{_id}?brief=true" : $"/{_id}";
            var json = await API.getMethod($"/product{query}");
            var book = JsonConvert.DeserializeObject<Book>(json);
            return book;
        }
        public async Task<String> getImageBook(string id)
        {
            var str = await API.getMethod($"/product/image/{id}");
            return str;
        }
        public async Task<List<Book>> getBookByCat(List<String> selectedItems)
        {
            String content = new String(" ");
            foreach(var item in selectedItems)
            {
                content = content + item+",";
            }
            var res = await API.getMethod($"/search/category?cats={content}");
            Debug.WriteLine(res);
            var bookList = JsonConvert.DeserializeObject<List<Book>>(res);
            return bookList;
            
        }
    }
}
