﻿using MyShopProject.DAO;
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
        public async Task<int> getSizeBookByCategoryAndPrice(List<String> selectecCat, List<int> selectedPrice)
        {
            int res = await book_DAO.getSizeofBooksByCatAndPrice(selectecCat,selectedPrice);
            return res;
        }
        public async Task<ObservableCollection<Book>> getBookByCategoryAndPricePagination(List<String> selectecCat,List<int> selectedPrice, int pageIndex,int limit)
        {
           
                var res = await book_DAO.getBookByCatAndPricePagination(selectecCat,selectedPrice,pageIndex,limit);
                return new ObservableCollection<Book>(res);
           
            
        }
        public async Task<int> countBookWithName(String content, List<int> selectedPrice)
        {
            var res = await book_DAO.countBookWithName(content, selectedPrice);
            return res;
        }
        public async Task<ObservableCollection<Book>> findBookWithName(String content,List<int> selectedPrice, int pageIndex, int limit)
        {
            var res =await book_DAO.findBookWithName(content,selectedPrice,pageIndex,limit);
            return new ObservableCollection<Book>(res);
        }
        
    }
}
