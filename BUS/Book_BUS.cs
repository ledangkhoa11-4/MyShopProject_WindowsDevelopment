using MyShopProject.DAO;
using MyShopProject.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShopProject.BUS
{
    public class Book_BUS
    {
        Book_DAO book_DAO;
        public Book_BUS()
        {
            book_DAO = new Book_DAO();
        }
        public async Task<ObservableCollection<Book>> getAllBook()
        {
            List<Book> res = await book_DAO.getAll();
            return new ObservableCollection<Book>(res);
        }
        public async Task<ObservableCollection<Book>> getAllBriefBook()
        {
            List<Book> res = await book_DAO.getAll(true);
            Debug.WriteLine(res.Count);
            return new ObservableCollection<Book>(res);
        }
        public async Task<String> getImageBook(string id)
        {
            try
            {
                string base64Encoded = await book_DAO.getImageBook(id);
                base64Encoded = base64Encoded.Remove(0, 1);
                base64Encoded = base64Encoded.Remove(base64Encoded.Length - 1, 1);
                return base64Encoded;
            }
            catch(Exception ex)
            {
                return "";
            }
            
        }
        public async Task<ObservableCollection<Book>> getBookByCategoryAndPrice(List<String> selectecCat,List<int> selectedPrice)
        {
           
                var res = await book_DAO.getBookByCatAndPrice(selectecCat,selectedPrice);
                return new ObservableCollection<Book>(res);
           
            
        }
    }
}
