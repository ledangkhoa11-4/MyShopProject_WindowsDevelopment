using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyShopProject.DAO
{
    public class API
    {
        private HttpClient _httpClient;
        public API(string uri)
        {
            _httpClient = new HttpClient{BaseAddress = new Uri(uri) };
        }
        public async Task<String> getMethod(string path)
        {
            var response = await _httpClient.GetAsync(path); //GET 127.0.0.1:5000/category
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
