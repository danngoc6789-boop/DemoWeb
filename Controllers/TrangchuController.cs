using DemoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoWeb.Controllers
{
    public class TrangchuController : Controller
    {
        private readonly AppDbContext db = new AppDbContext();
        // GET: Trangchu
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        // POST: /Trangchu/ContactSend
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactSend(string name, string phone, string message)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrWhiteSpace(name) ||
                    string.IsNullOrWhiteSpace(phone) ||
                    string.IsNullOrWhiteSpace(message))
                {
                    TempData["Error"] = "Vui lòng điền đầy đủ thông tin!";
                    return RedirectToAction("Contact");
                }

                // Tạo SupportRequest mới
                var supportRequest = new SupportRequest
                {
                    CustomerName = name.Trim(),
                    Phone = phone.Trim(),
                    Message = message.Trim(),
                    Email = "", // Có thể để trống vì không required
                    Status = "Chờ xử lý",
                    CreatedAt = DateTime.Now
                };

                // Lưu vào database
                db.SupportRequests.Add(supportRequest);
                db.SaveChanges();

                // Thông báo thành công
                TempData["Success"] = "Cảm ơn bạn đã liên hệ! Chúng tôi sẽ phản hồi sớm nhất.";

                return RedirectToAction("Contact");
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                TempData["Error"] = "Có lỗi xảy ra. Vui lòng thử lại sau!";
                return RedirectToAction("Contact");
            }
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