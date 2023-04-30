using MyShopProject.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShopProject.DAO
{
    public class Report_DAO
    {
        public async Task<List<StatisticsProductByTime>> statisticByRangeDate(String start, String end, String id)
        {
            var json = await API.getMethod($"/report/statistic/day?start={start}&end={end}&id={id}");
            var statistics = JsonConvert.DeserializeObject<List<StatisticsProductByTime>>(json);
            return statistics;
        }
        public async Task<List<StatisticsProductByTime>> statisticByMonth(int month, int year, String id)
        {
            var json = await API.getMethod($"/report/statistic/month?month={month}&year={year}&id={id}");
            var statistics = JsonConvert.DeserializeObject<List<StatisticsProductByTime>>(json);
            return statistics;
        }
        public async Task<List<StatisticsProductByTime>> statisticByYear(int year, String id)
        {
            var json = await API.getMethod($"/report/statistic/year?year={year}&id={id}");
            var statistics = JsonConvert.DeserializeObject<List<StatisticsProductByTime>>(json);
            return statistics;
        }
        public async Task<List<Profit>> statisticProfitByRangeDate(String start, String end)
        {
            var json = await API.getMethod($"/report/profit/day?start={start}&end={end}");
            var profit = JsonConvert.DeserializeObject<List<Profit>>(json);
            return profit;
        }
        public async Task<List<Profit>> statisticProfitByMonth(int month, int year)
        {
            var json = await API.getMethod($"/report/profit/month?month={month}&year={year}");
            var profit = JsonConvert.DeserializeObject<List<Profit>>(json);
            return profit;
        }
        public async Task<List<Profit>> statisticProfitByYear(int year)
        {
            var json = await API.getMethod($"/report/profit/year?year={year}");
            var profit = JsonConvert.DeserializeObject<List<Profit>>(json);
            return profit;
        }
    }
}
