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
        public async Task<String> getTotal()
        {
            var json = await API.getMethod($"/order/count");
            return json;
        }
        public async Task<String> addNewOrder(string content)
        {
            var json = await API.postMethod("/order", content);
            return json;
        }
        public async Task<String> updateOrder(string id, string content)
        {
            var json = await API.postMethod($"/order/update/{id}", content);
            return json;
        }
        public async Task<String> deleteOrder(string id)
        {
            var json = await API.getMethod($"/order/delete/{id}");
            return json;
        }
    }
}
