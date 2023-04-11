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
        public async Task<ObservableCollection<Order>> getAllOrder(int limit = 6, int skip = 0)
        {
            var listOrder = await order_DAO.getAll(limit,skip);
            if (listOrder == null) return new ObservableCollection<Order>();
            foreach(Order order in listOrder)
            {
                var listCart = order.DetailCart;
                if (order.Coupon != null && order.Coupon._id != null)
                    order.Coupon = MainWindow.modelBinding.listCoupon.FirstOrDefault(coupon => coupon._id == order.Coupon._id);
                else
                    order.Coupon = null;
                foreach(DetailOrder cart in listCart)
                {
                    string id = cart.Book._id;
                    var book = await book_DAO.get(id, true);
                    cart.Book = book;

                }
            }
            return new ObservableCollection<Order>(listOrder);
        }
        public async Task<int> getCountOrder()
        {
            string json = await order_DAO.getTotal();
            return int.Parse(json);
        }
        public async Task<String> deletetOrder(Order order)
        {
            string json = await order_DAO.deleteOrder(order._id);
            return json;
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
        public async Task<String> UpdateOrder(Order newOrder)
        {
            string id = newOrder._id;
            var jsonData = JsonConvert.SerializeObject(newOrder);
            JObject jsonObj = JObject.Parse(jsonData);
            jsonObj.Remove("_id");
            try
            {
                JObject couponInfo = jsonObj["Coupon"].ToObject<JObject>();
                if (couponInfo != null)
                {
                    couponInfo.Remove("DateAdd");
                    couponInfo.Remove("Name");
                    couponInfo.Remove("DiscountPercent");
                }

            }
            catch (Exception ex)
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
            return await order_DAO.updateOrder(id,newJsonString);
        }
    }
}
