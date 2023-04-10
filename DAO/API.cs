using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MyShopProject.DAO
{
    public class API
    {
        private static HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://127.0.0.1:5000") };

        public static async Task<Tuple<Boolean, String>> testConnection()
        {
            try
            {
                var response = await _httpClient.GetAsync("/"); //GET 127.0.0.1:5000/category
                var stringResult = await response.Content.ReadAsStringAsync();
                return Tuple.Create<Boolean,String>(true, "Connect web service successfully");
            }
            catch (Exception ex)
            {
                return Tuple.Create<Boolean, String>(false, ex.Message); 
            }
        }

        public static async Task<String> getMethod(string path)
        {
            var response = await _httpClient.GetAsync(path); //GET 127.0.0.1:5000/category
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        public static async Task<String> postMethod(string path, string content)
        {
            var data = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(path, data);
            string result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}
