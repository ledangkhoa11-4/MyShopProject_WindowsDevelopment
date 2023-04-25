﻿using MyShopProject.DAO;
using MyShopProject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShopProject.BUS
{
    public class Report_BUS
    {
        Report_DAO report_DAO;
        public Report_BUS()
        {
            report_DAO = new Report_DAO();
        }
        public async Task<List<StatisticsProductByTime>> statisticProductByDate(String from, String to, String id)
        {
            var result = await report_DAO.statisticByRangeDate(from, to, id);
            return result;
        }
        public async Task<List<StatisticsProductByTime>> statisticProductByMonth(int month, int year, String id)
        {
            var result = await report_DAO.statisticByMonth(month, year, id);
            return result;
        }
        public async Task<List<Profit>> statisticProfitByDate(String from, String to)
        {
            var result = await report_DAO.statisticProfitByRangeDate(from, to);
            return result;
        }
        public async Task<List<Profit>> statisticProfitByMonth(int month, int year)
        {
            List<Profit> result = await report_DAO.statisticProfitByMonth(month, year);
            return result;
        }
    }
}
