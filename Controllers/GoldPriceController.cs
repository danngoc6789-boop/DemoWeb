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
                var goldPriceService = new GoldPriceDbService();

                // Lấy dữ liệu từ database
                var prices = await goldPriceService.GetGoldPricesFromDbAsync();

                // Lấy thời gian cập nhật
                var lastUpdate = await goldPriceService.GetLastUpdateTimeAsync();
                ViewBag.LastUpdate = lastUpdate;

                // Nhóm theo ProductName để loại bỏ trùng lặp (nếu cần)
                // Nếu muốn chỉ hiển thị dữ liệu mới nhất
                var latestPrices = prices
                    .GroupBy(p => p.ProductName)
                    .Select(g => g.OrderByDescending(p => p.CreatedAt).First())
                    .OrderBy(p => p.ProductName)
                    .ToList();

                return View(latestPrices);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Không thể tải giá vàng: " + ex.Message;
                return View(new List<GoldPrice>());
            }
           
        }
    }
}