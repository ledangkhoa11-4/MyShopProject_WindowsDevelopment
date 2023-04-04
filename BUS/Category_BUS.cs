﻿using MyShopProject.DAO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Category = MyShopProject.DTO.Category;
namespace MyShopProject.BUS
{
    public class Category_BUS
    {
        Category_DAO category_DAO;
        public Category_BUS()
        {
            category_DAO = new Category_DAO();
        }
        public async Task<ObservableCollection<Category>> getAllCategory()
        {
            List<Category> res = await category_DAO.getAll();
            return new ObservableCollection<Category>(res);
        }
    }
}