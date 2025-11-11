using DemoWeb.Controllers;
using DemoWeb.Models;
using DemoWeb.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Services.Description;

namespace DemoWeb.Services
{
    public class GoldPriceDbService
    {
        private readonly GoldBtmcService _apiService;

        public GoldPriceDbService()
        {
            _apiService = new GoldBtmcService();
        }
        /// Lấy thời gian hiện tại theo múi giờ Việt Nam (UTC+7)
       
        private DateTime GetVietnamTime()
        {
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);
        }
        /// Lấy dữ liệu từ API, xóa dữ liệu cũ và lưu dữ liệu mới

        public async Task<List<GoldPrice>> UpdateGoldPricesAsync()
        {
            try
            {
                Debug.WriteLine("=== BẮT ĐẦU CẬP NHẬT GIÁ VÀNG ===");

                // 1. Lấy dữ liệu mới từ API
                var newPrices = await _apiService.FetchAsync();

                if (newPrices == null || newPrices.Count == 0)
                {
                    throw new Exception("Không thể lấy dữ liệu từ API");
                }

                Debug.WriteLine($"Đã lấy {newPrices.Count} mục giá vàng từ API");

                // 2. Xóa dữ liệu cũ và lưu dữ liệu mới
                using (var db = new AppDbContext())
                {
                    // Xóa TẤT CẢ dữ liệu giá vàng cũ
                    Debug.WriteLine("Đang xóa dữ liệu cũ...");
                    db.Database.ExecuteSqlCommand("DELETE FROM GoldPrices");

                    // Set CreatedAt cho tất cả items mới
                    var now = DateTime.Now;
                    foreach (var price in newPrices)
                    {
                        price.CreatedAt = now;
                    }

                    // Thêm dữ liệu mới
                    Debug.WriteLine("Đang lưu dữ liệu mới...");
                    db.GoldPrices.AddRange(newPrices);

                    // Lưu vào database
                    await db.SaveChangesAsync();
                    Debug.WriteLine("=== CẬP NHẬT THÀNH CÔNG ===");
                }

                return newPrices;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"=== LỖI KHI CẬP NHẬT: {ex.Message} ===");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        /// Lấy giá vàng từ database
        public async Task<List<GoldPrice>> GetGoldPricesFromDbAsync()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var prices = await db.GoldPrices
                        .OrderBy(g => g.Id)
                        .ToListAsync();

                    Debug.WriteLine($"Đã lấy {prices.Count} mục từ database");
                    return prices;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"=== LỖI KHI LẤY TỪ DB: {ex.Message} ===");
                throw;
            }
        }

        /// Kiểm tra xem dữ liệu có cũ không (hơn 30 phút)

        public async Task<bool> IsDataStaleAsync(int minutesThreshold = 30)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var latestRecord = await db.GoldPrices
                        .OrderByDescending(g => g.CreatedAt)
                        .FirstOrDefaultAsync();

                    // Nếu không có data trong DB → cần update
                    if (latestRecord == null)
                    {
                        Debug.WriteLine("Không có dữ liệu trong DB → Cần cập nhật");
                        return true;
                    }

                    // Kiểm tra thời gian
                    var minutesPassed = (DateTime.Now - latestRecord.CreatedAt).TotalMinutes;
                    Debug.WriteLine($"Dữ liệu trong DB đã {minutesPassed:F1} phút tuổi (ngưỡng: {minutesThreshold} phút)");

                    return minutesPassed > minutesThreshold;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"=== LỖI KHI KIỂM TRA: {ex.Message} ===");
                // Nếu có lỗi, coi như cần update để an toàn
                return true;
            }
        }
        /// Lấy thời gian cập nhật gần nhất
        
        public async Task<DateTime?> GetLastUpdateTimeAsync()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var latestRecord = await db.GoldPrices
                        .OrderByDescending(g => g.CreatedAt)
                        .FirstOrDefaultAsync();

                    return latestRecord?.CreatedAt;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}

