using MyShopProject.DAO;
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
    }
}
