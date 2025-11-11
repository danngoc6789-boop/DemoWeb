using DemoWeb.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

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

                var json = JObject.Parse(response);
                var dataList = json["DataList"]?["Data"];

                if (dataList == null)
                    throw new Exception("Không tìm thấy dữ liệu trong API BTMC");

                Debug.WriteLine($"=== BẮT ĐẦU XỬ LÝ {dataList.Count()} ITEMS ===");

                foreach (var item in dataList)
                {
                    // Lấy số thứ tự từ @row
                    string rowNumber = (string)item["@row"];

                    if (string.IsNullOrEmpty(rowNumber))
                        continue;

                    // Tạo tên field động dựa trên số row
                    string productName = (string)item[$"@n_{rowNumber}"];
                    string karat = (string)item[$"@k_{rowNumber}"];
                    string purity = (string)item[$"@h_{rowNumber}"];
                    string buyPriceStr = (string)item[$"@pb_{rowNumber}"];
                    string sellPriceStr = (string)item[$"@ps_{rowNumber}"];
                    string dateStr = (string)item[$"@d_{rowNumber}"];

                    Debug.WriteLine($"Row {rowNumber}: {productName} - Mua: {buyPriceStr} - Bán: {sellPriceStr}");

                    list.Add(new GoldPrice
                    {
                        ProductName = productName,
                        Karat = karat,
                        Purity = purity,
                        BuyPrice = decimal.TryParse(buyPriceStr, out var pb) ? pb : 0,
                        SellPrice = decimal.TryParse(sellPriceStr, out var ps) ? ps : 0,
                        Timestamp = DateTime.TryParse(dateStr, out var dt) ? dt : DateTime.Now,
                        Source = apiUrl,
                        
                    });
                }

                Debug.WriteLine($"=== HOÀN THÀNH: Đã thêm {list.Count} items ===");
            }
            return list;
        }
    }
}