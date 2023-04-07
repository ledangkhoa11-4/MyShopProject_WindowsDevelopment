using MyShopProject.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShopProject.DAO
{
    public class Order_DAO
    {
        public async Task<List<Order>> getAll(int limit, int offset)
        {
            var json = await API.getMethod($"/order?limit={limit}&offset={offset}");
            var orderList = JsonConvert.DeserializeObject<List<Order>>(json);
            return orderList;
        }
        public async Task<String> addNewOrder(string content)
        {
            var json = await API.postMethod("/order", content);
            return json;
        }
    }
}
