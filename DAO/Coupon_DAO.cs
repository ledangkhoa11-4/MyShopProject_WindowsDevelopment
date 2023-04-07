using MyShopProject.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShopProject.DAO
{
    public class Coupon_DAO
    {
        public async Task<List<Coupon>> getAll()
        {
            var json = await API.getMethod("/coupon");
            var couponList = JsonConvert.DeserializeObject<List<Coupon>>(json);
            return couponList;
        }
        public async Task<String> addNewCoupon(string content)
        {
            var json = await API.postMethod("/coupon", content);
            return json;
        }
    }
}
