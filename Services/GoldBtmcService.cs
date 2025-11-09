using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DemoWeb.Models;
using Newtonsoft.Json.Linq;

namespace DemoWeb.Services
{
    public class GoldBtmcService
    {
        private readonly string apiUrl = "http://api.btmc.vn/api/BTMCAPI/getpricebtmc?key=3kd8ub1llcg9t45hnoh8hmn7t5kc2v";

        public async Task<List<GoldPrice>> FetchAsync()
        {
            var list = new List<GoldPrice>();

            using (var client = new HttpClient())
            {
                string response = await client.GetStringAsync(apiUrl);
                response = response.Trim();

                // Parse JSON
                var json = JObject.Parse(response);
                var dataList = json["DataList"]?["Data"];

                if (dataList == null)
                    throw new Exception("Không tìm thấy dữ liệu trong API BTMC");

                foreach (var item in dataList)
                {
                    list.Add(new GoldPrice
                    {
                        ProductName = (string)item["@n_1"],
                        Karat = (string)item["@k_1"],
                        Purity = (string)item["@h_1"],
                        BuyPrice = decimal.TryParse((string)item["@pb_1"], out var pb) ? pb : 0,
                        SellPrice = decimal.TryParse((string)item["@ps_1"], out var ps) ? ps : 0,
                        Timestamp = DateTime.TryParse((string)item["@d_1"], out var dt) ? dt : DateTime.Now,
                        Source = apiUrl
                    });
                }
            }

            return list;
        }
    }
}
