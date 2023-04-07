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
        public Order_BUS()
        {
            order_DAO = new Order_DAO();
        }
        public async Task<ObservableCollection<Order>> getAllOrder()
        {
            List<Order> res = await order_DAO.getAll();
            return new ObservableCollection<Order>(res);
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
                    string couponId = (string)couponInfo["_id"];
                    JValue newCouponValue = new JValue(couponId);
                    jsonObj["Coupon"].Replace(newCouponValue);
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
                JValue newBookValue = new JValue(bookId);
                item["Book"].Replace(newBookValue);
 
            }
            jsonObj["DetailCart"] = jsonArray;
            string newJsonString = jsonObj.ToString();
            Debug.WriteLine(newJsonString);
            return await order_DAO.addNewOrder(newJsonString);
        }
    }
}
