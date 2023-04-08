using MyShopProject.DAO;
using MyShopProject.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace MyShopProject.BUS
{
    public class Order_BUS
    {
        Order_DAO order_DAO;
        Book_DAO book_DAO;
        public Order_BUS()
        {
            order_DAO = new Order_DAO();
            book_DAO = new Book_DAO();
        }
        public async Task<ObservableCollection<Order>> getAllOrder(int limit = 6, int offset = 0)
        {
            var listOrder = await order_DAO.getAll(limit,offset);
            foreach(Order order in listOrder)
            {
                var listCart = order.DetailCart;
                foreach(DetailOrder cart in listCart)
                {
                    string id = cart.Book._id;
                    var book = await book_DAO.get(id, true);
                    cart.Book = book;
                }
            }
            return new ObservableCollection<Order>(listOrder);
        }
        public async Task<String> AddOrder(Order newOrder)
        {
            var jsonData = JsonConvert.SerializeObject(newOrder);
            JObject jsonObj = JObject.Parse(jsonData);
            jsonObj.Remove("_id");
            try
            {
                JObject couponInfo = jsonObj["Coupon"].ToObject<JObject>();
                if(couponInfo != null )
                {
                    couponInfo.Remove("DateAdd");
                    couponInfo.Remove("Name");
                    couponInfo.Remove("DiscountPercent");
                }
                
            }
            catch(Exception ex)
            {
                //ignore
            }
            JArray jsonArray = (JArray)jsonObj["DetailCart"];
            foreach (JObject item in jsonArray)
            {
                JObject bookInfo = item["Book"].ToObject<JObject>();
                string bookId = (string)bookInfo["_id"];
                JObject newBookObj = new JObject(new JProperty("_id", bookId));
                item["Book"].Replace(newBookObj);
            }
            jsonObj["DetailCart"] = jsonArray;
            string newJsonString = jsonObj.ToString();
            return await order_DAO.addNewOrder(newJsonString);
        }
    }
}
