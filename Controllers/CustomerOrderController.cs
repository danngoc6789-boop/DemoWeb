using DemoWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoWeb.Controllers
{
    public class CustomerOrderController : Controller
    {
        private AppDbContext db = new AppDbContext();
        // GET: CustomerOrders
        [Authorize] // Yêu cầu đăng nhập
        public ActionResult Index(string status)
        {
            // Lấy username của user đang đăng nhập
            string username = User.Identity.Name;

            // Lấy tất cả đơn hàng của customer này
            var orders = db.Orders
                .Where(o => o.Username == username)
                .OrderByDescending(o => o.OrderDate);
            // Lọc theo trạng thái (giữ nguyên không cần map vì Admin lưu tiếng Việt)
            if (!string.IsNullOrEmpty(status))
            {
                orders = (IOrderedQueryable<Order>)orders.Where(o => o.Status == status);
            }

            ViewBag.CurrentStatus = status;
            return View(orders.ToList());

        }
        
      
        // GET: CustomerOrders/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var order = db.Orders.Find(id);

            if (order == null)
            {
                return HttpNotFound();
            }

            // Kiểm tra xem đơn hàng có phải của user đang đăng nhập không
            if (order.Username != User.Identity.Name)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }

            // Lấy chi tiết đơn hàng
            var orderDetails = db.OrderDetails
                .Where(od => od.OrderId == id)
                .Include("Product")
                .ToList();

            ViewBag.OrderDetails = orderDetails;

            return View(order);
        }

        // POST: CustomerOrders/Cancel/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(int id)
        {
            var order = db.Orders.Find(id);

            if (order == null)
            {
                return HttpNotFound();
            }

            // Kiểm tra quyền sở hữu
            if (order.Username != User.Identity.Name)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }

            // Chỉ cho phép hủy đơn hàng đang chờ xử lý
            if (order.Status == "Pending" || order.Status == "Đang xử lý")
            {
                order.Status = "Cancelled";
                order.CancelledDate = DateTime.Now;
                db.SaveChanges();

                TempData["SuccessMessage"] = "Đơn hàng đã được hủy thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Không thể hủy đơn hàng này!";
            }

            return RedirectToAction("Details", new { id = id });
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