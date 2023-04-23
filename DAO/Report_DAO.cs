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
    }
}
