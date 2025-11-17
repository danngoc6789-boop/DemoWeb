using DemoWeb.Models;
using DemoWeb.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Globalization;

namespace DemoWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private AppDbContext db = new AppDbContext();
        private GoldPriceDbService _goldPriceService = new GoldPriceDbService();

        [Authorize(Roles = "Admin")]
        // GET: Admin
        public async Task<ActionResult> Index()
        {
            
                var today = DateTime.Today;
                var thisMonth = new DateTime(today.Year, today.Month, 1);

                ViewBag.TotalProducts = await db.Products.CountAsync();
                ViewBag.TotalOrders = await db.Orders.CountAsync();
                ViewBag.PendingOrders = 0; // Chưa có Status trong Order model
                ViewBag.TodayOrders = await db.Orders.CountAsync(o => DbFunctions.TruncateTime(o.OrderDate) == today);

                ViewBag.MonthRevenue = await db.Orders
                    .Where(o => o.OrderDate >= thisMonth)
                    .SumAsync(o => (decimal?)o.TotalAmount) ?? 0;

                ViewBag.RecentOrders = await db.Orders
                    .OrderByDescending(o => o.OrderDate)
                    .Take(5)
                    .ToListAsync();

              

                return View();
            }

            // ==================== QUẢN LÝ SẢN PHẨM ====================

            // GET: Admin/Products
            public async Task<ActionResult> Products(string search, string category, int page = 1)
            {
                int pageSize = 20;
                var query = db.Products.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
                    ViewBag.Search = search;
                }

                if (!string.IsNullOrEmpty(category))
                {
                    query = query.Where(p => p.Category == category);
                    ViewBag.Category = category;
                }

                var totalItems = await query.CountAsync();
                var products = await query
                    .OrderByDescending(p => p.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                ViewBag.TotalItems = totalItems;

                ViewBag.Categories = await db.Products
                    .Select(p => p.Category)
                    .Distinct()
                    .ToListAsync();

                return View(products);
            }

            // GET: Admin/Details/5
            public async Task<ActionResult> Details(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Product product = await db.Products.FindAsync(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                return View(product);
            }

            // GET: Admin/Create
            public async Task<ActionResult> Create()
            {
                ViewBag.Categories = await db.Products
                    .Select(p => p.Category)
                    .Distinct()
                    .ToListAsync();

                return View();
            }

            // POST: Admin/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> Create([Bind(Include = "Id,Name,Description,Price,Category,Image,Images,Gender,Type")] Product product, HttpPostedFileBase imageFile)
            {
                if (ModelState.IsValid)
                {
                    // Xử lý upload ảnh
                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        var uploadPath = Server.MapPath("~/Content/images/products/");

                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }

                        var filePath = Path.Combine(uploadPath, fileName);
                        imageFile.SaveAs(filePath);
                        product.Image = "~/Content/images/products/" + fileName;
                        product.Images = "~/Content/images/products/" + fileName;
                    }

                    product.CreatedDate = DateTime.Now;
                    db.Products.Add(product);
                    await db.SaveChangesAsync();

                    TempData["SuccessMessage"] = "✓ Đã thêm sản phẩm thành công!";
                    return RedirectToAction("Products");
                }

                ViewBag.Categories = await db.Products
                    .Select(p => p.Category)
                    .Distinct()
                    .ToListAsync();

                return View(product);
            }

            // GET: Admin/Edit/5
            public async Task<ActionResult> Edit(int? id)
            {
                if (id == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy sản phẩm!";
                    return RedirectToAction("Products");
                }

                Product product = await db.Products.FindAsync(id);
                if (product == null)
                {
                    TempData["ErrorMessage"] = "Sản phẩm không tồn tại!";
                    return RedirectToAction("Products");
                }

                ViewBag.Categories = await db.Products
                    .Select(p => p.Category)
                    .Distinct()
                    .ToListAsync();

                return View(product);
            }

            // POST: Admin/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Description,Price,Category,Image,Images,Gender,Type,CreatedDate")] Product product, HttpPostedFileBase imageFile)
            {
                if (ModelState.IsValid)
                {
                    // Xử lý upload ảnh mới
                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        var uploadPath = Server.MapPath("~/Content/images/products/");

                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }

                        var filePath = Path.Combine(uploadPath, fileName);
                        imageFile.SaveAs(filePath);
                        product.Image = "~/Content/images/products/" + fileName;
                        product.Images = "~/Content/images/products/" + fileName;
                    }

                    db.Entry(product).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    TempData["SuccessMessage"] = "✓ Đã cập nhật sản phẩm thành công!";
                    return RedirectToAction("Products");
                }

                ViewBag.Categories = await db.Products
                    .Select(p => p.Category)
                    .Distinct()
                    .ToListAsync();

                return View(product);
            }

            // GET: Admin/Delete/5
            public async Task<ActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Product product = await db.Products.FindAsync(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                return View(product);
            }

            // POST: Admin/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> DeleteConfirmed(int id)
            {
                Product product = await db.Products.FindAsync(id);
                db.Products.Remove(product);
                await db.SaveChangesAsync();

                TempData["SuccessMessage"] = "✓ Đã xóa sản phẩm thành công!";
                return RedirectToAction("Products");
            }

            // POST: Admin/DeleteProduct (AJAX)
            [HttpPost]
            public async Task<JsonResult> DeleteProduct(int id)
            {
                try
                {
                    var product = await db.Products.FindAsync(id);
                    if (product != null)
                    {
                        db.Products.Remove(product);
                        await db.SaveChangesAsync();
                        return Json(new { success = true, message = "✓ Đã xóa sản phẩm thành công!" });
                    }

                    return Json(new { success = false, message = "Không tìm thấy sản phẩm!" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Lỗi: " + ex.Message });
                }
            }

            // ==================== QUẢN LÝ ĐƠN HÀNG ====================

            // GET: Admin/Orders
            public async Task<ActionResult> Orders(string search, int page = 1)
            {
                int pageSize = 20;
                var query = db.Orders.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(o => o.CustomerName.Contains(search) ||
                                             o.Phone.Contains(search) ||
                                             o.Email.Contains(search) ||
                                             o.OrderCode.Contains(search));
                    ViewBag.Search = search;
                }

                var totalItems = await query.CountAsync();
                var orders = await query
                    .OrderByDescending(o => o.OrderDate)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                ViewBag.TotalItems = totalItems;

                return View(orders);
            }

            // GET: Admin/OrderDetail/5
            public async Task<ActionResult> OrderDetail(int? id)
            {
                var order = await db.Orders
                    .Include(o => o.OrderDetails)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                {
                    return HttpNotFound();
                }

                return View(order);
            }
           // cập nhật trạng thái đơn
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult UpdateStatus(int orderId, string status)
            {
                var order = db.Orders.Find(orderId);
                if (order != null)
                {
                    order.Status = status;
                    db.SaveChanges();
                }
                return RedirectToAction("Orders");
            }

        // ==================== QUẢN LÝ GIÁ VÀNG ====================

        // GET: Admin/GoldPrices
        public async Task<ActionResult> GoldPrices()
            {
                try
                {
                    var prices = await _goldPriceService.GetGoldPricesFromDbAsync();
                    var lastUpdate = await _goldPriceService.GetLastUpdateTimeAsync();
                    var isStale = await _goldPriceService.IsDataStaleAsync(30);

                    ViewBag.LastUpdate = lastUpdate;
                    ViewBag.IsStale = isStale;
                    ViewBag.MinutesOld = lastUpdate.HasValue
                        ? (DateTime.Now - lastUpdate.Value).TotalMinutes
                        : 0;

                    return View(prices);
                }
                catch
                {
                    ViewBag.LastUpdate = null;
                    ViewBag.IsStale = true;
                    ViewBag.MinutesOld = 0;
                    return View(new List<GoldPrice>());
                }
            }

            // GET: Admin/UpdateGoldPrice
            public async Task<ActionResult> UpdateGoldPrice()
            {
                try
                {
                    var prices = await _goldPriceService.UpdateGoldPricesAsync();
                    TempData["SuccessMessage"] = $"✓ Đã cập nhật {prices.Count} mục giá vàng lúc {DateTime.Now:HH:mm:ss dd/MM/yyyy}";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"✗ Lỗi: {ex.Message}";
                }

                return RedirectToAction("GoldPrices");
            }

        // ==================== BÁO CÁO THỐNG KÊ ====================

        // GET: Admin/Reports
        public async Task<ActionResult> Reports(DateTime? fromDate, DateTime? toDate, string type = "day")
        {
            // Nếu chưa có từ ngày, đến ngày → mặc định 1 tháng gần đây
            if (!fromDate.HasValue) fromDate = DateTime.Today.AddMonths(-1);
            if (!toDate.HasValue) toDate = DateTime.Today;

            ViewBag.FromDate = fromDate.Value.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate.Value.ToString("yyyy-MM-dd");
            ViewBag.Type = type ?? "day";

            // Dùng o.OrderDate < toDate.AddDays(1) (đến 23:59:59)
            var endDate = toDate.Value.AddDays(1);

            var orders = await db.Orders
                .Include(o => o.OrderDetails.Select(od => od.Product))
                .Where(o => o.OrderDate >= fromDate && o.OrderDate < endDate)
                .ToListAsync();

            // 2. Thống kê tổng quan
            ViewBag.TotalOrders = orders.Count;
            ViewBag.DeliveredOrders = orders.Count(o => o.Status == "Đã giao");
            ViewBag.PendingOrders = orders.Count(o => o.Status == "Chưa xử lý");
            ViewBag.CancelledOrders = orders.Count(o => o.Status == "Đã hủy");
            ViewBag.TotalRevenue = orders.Sum(o => (decimal?)o.TotalAmount) ?? 0;

            // 3. Doanh thu theo ngày hoặc tháng
            if (type == "month")
            {
                var monthly = orders
                    .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                    .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                    .Select(g => new
                    {
                        Label = $"Tháng {g.Key.Month}/{g.Key.Year}",
                        Total = g.Sum(o => (decimal?)o.TotalAmount) ?? 0
                    }).ToList();

                ViewBag.ChartLabels = monthly.Select(m => m.Label).ToList();
                ViewBag.ChartData = monthly.Select(m => m.Total).ToList();
            }
            else
            {
                var daily = orders
                    .GroupBy(o => o.OrderDate.Date)
                    .OrderBy(g => g.Key)
                    .Select(g => new
                    {
                        Label = g.Key.ToString("yyyy-MM-dd"),
                        Total = g.Sum(o => (decimal?)o.TotalAmount) ?? 0
                    }).ToList();

                ViewBag.ChartLabels = daily.Select(d => d.Label).ToList();
                ViewBag.ChartData = daily.Select(d => d.Total).ToList();
            }

            // 4. Top 10 sản phẩm bán chạy
            var topProducts = orders
                .SelectMany(o => o.OrderDetails)
                .GroupBy(od => od.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    ProductName = g.First().Product.Name,
                    TotalQuantity = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(p => p.TotalQuantity)
                .Take(10)
                .ToList();

            ViewBag.TopProducts = topProducts;
       

            return View(orders);
        }

        // ==================== API trả Top 10 sản phẩm theo khoảng ngày ====================
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> GetTopProducts(DateTime fromDate, DateTime toDate)
        {
            // ✅ SỬA: Cũng áp dụng cách sửa ngày ở đây
            var endDate = toDate.AddDays(1);

            var orders = await db.Orders
                .Include(o => o.OrderDetails.Select(od => od.Product))
                .Where(o => o.OrderDate >= fromDate && o.OrderDate < endDate)
                .ToListAsync();

            var topProducts = orders
                .SelectMany(o => o.OrderDetails)
                .GroupBy(od => od.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    ProductName = g.First().Product.Name,
                    TotalQuantity = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(p => p.TotalQuantity)
                .Take(10)
                .ToList();

            System.Diagnostics.Debug.WriteLine($"🎯 GetTopProducts - Orders: {orders.Count}, TopProducts: {topProducts.Count}");

            return Json(topProducts, JsonRequestBehavior.AllowGet);
        }
        // ==================== QUẢN LÝ CSKH ====================

        public async Task<ActionResult> Supports(int page = 1)
        {
            int pageSize = 20;

            var query = db.SupportRequests.OrderByDescending(s => s.CreatedAt);

            var totalItems = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return View(items);
        }
        public async Task<ActionResult> SupportDetail(int id)
        {
            var item = await db.SupportRequests.FindAsync(id);
            if (item == null) return HttpNotFound();

            return View(item);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateSupportStatus(int id, string status)
        {
            var item = await db.SupportRequests.FindAsync(id);
            if (item != null)
            {
                item.Status = status;
                await db.SaveChangesAsync();
                TempData["SuccessMessage"] = "✓ Đã cập nhật trạng thái thành công!";
            }

            // Sửa từ "Support" thành "SupportDetail" 
            return RedirectToAction("SupportDetail", new { id = id });
        }
        
        [HttpPost]
        public async Task<ActionResult> DeleteSupport(int id)
        {
            var item = await db.SupportRequests.FindAsync(id);
            if (item != null)
            {
                db.SupportRequests.Remove(item);
                await db.SaveChangesAsync();
                TempData["SuccessMessage"] = "✓ Đã xóa yêu cầu hỗ trợ!";
            }

            // Sửa từ "Support" thành "Supports"
            return RedirectToAction("Supports");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
