using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DemoWeb.Services;
using DemoWeb.Models;
using System.Diagnostics;

namespace DemoWeb.Controllers
{
    public class GoldPriceController : Controller
    {
        // GET: GoldPrice/BangGia
        public async Task<ActionResult> BangGia()
        {
            try
            {
                Debug.WriteLine("=== BẮT ĐẦU GỌI API BTMC ===");

                var goldService = new GoldBtmcService();
                var goldPrices = await goldService.FetchAsync();

                Debug.WriteLine($"=== ĐÃ LẤY ĐƯỢC {goldPrices.Count} MỤC GIÁ VÀNG ===");

                foreach (var item in goldPrices.Take(3))
                {
                    Debug.WriteLine($"Item: {item.ProductName} - Mua: {item.BuyPrice} - Bán: {item.SellPrice}");
                }

                return View(goldPrices);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"=== LỖI KHI GỌI API: {ex.Message} ===");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");

                ViewBag.Error = $"Lỗi khi lấy dữ liệu: {ex.Message}";
                return View(new List<GoldPrice>());
            }
        }
    }
}