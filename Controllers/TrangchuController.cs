using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoWeb.Controllers
{
    public class TrangchuController : Controller
    {
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
        public ActionResult ContactSend(string name, string phone, string message)
        {
            // Sau này bạn có thể:
            // - Gửi email
            // - Lưu vào DB
            // - Gửi Telegram / Zalo / v.v.

            // Tạm thời mình chỉ trả ra nội dung xác nhận.
            // Bạn có thể thay bằng redirect quay lại Contact với TempData nếu muốn.
            return Content(
                "Đã nhận thông tin liên hệ từ: "
                + name + " / " + phone + " - Nội dung: " + message
            );
        }
    }
}