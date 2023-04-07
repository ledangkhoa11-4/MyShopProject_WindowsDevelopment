using MyShopProject.DAO;
using MyShopProject.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShopProject.BUS
{
    public class Coupon_BUS
    {
        Coupon_DAO coupon_DAO;
        public Coupon_BUS()
        {
            coupon_DAO = new Coupon_DAO();
        }
        public async Task<ObservableCollection<Coupon>> getAllCoupon()
        {
            List<Coupon> res = await coupon_DAO.getAll();
            return new ObservableCollection<Coupon>(res);
        }
        public async Task<String> AddCoupon(Coupon newCoupon)
        {
            var jsonData = JsonConvert.SerializeObject(newCoupon);
            return await coupon_DAO.addNewCoupon(jsonData);
        }
    }
}
