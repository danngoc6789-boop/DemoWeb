using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DemoWeb.Models;

namespace DemoWeb.Controllers
{
    public class CartController : Controller
    {
        // Lấy cart từ Session, nếu chưa có thì tạo list rỗng
        private List<CartItem> GetCart()
        {
            if (Session["Cart"] == null)
            {
                Session["Cart"] = new List<CartItem>();
            }
            return (List<CartItem>)Session["Cart"];
        }

        // GET: /Cart/
        public ActionResult Index()
        {
            var cart = GetCart();

            // tổng số lượng sp trong giỏ
            int itemCount = cart.Sum(x => x.Quantity);

            // tổng tiền
            decimal total = cart.Sum(x => x.ThanhTien);

            ViewBag.ItemCount = itemCount;
            ViewBag.TotalAmount = total;

            return View(cart);
        }

        // Tăng số lượng 1 sản phẩm
        // /Cart/Plus/5
        public ActionResult Plus(int id)
        {
            var cart = GetCart();
            var line = cart.FirstOrDefault(x => x.ProductId == id);
            if (line != null)
            {
                line.Quantity += 1;
            }
            return RedirectToAction("Index");
        }

        // Giảm số lượng 1 sản phẩm (tối thiểu 1)
        // /Cart/Minus/5
        public ActionResult Minus(int id)
        {
            var cart = GetCart();
            var line = cart.FirstOrDefault(x => x.ProductId == id);
            if (line != null)
            {
                line.Quantity = Math.Max(1, line.Quantity - 1);
            }
            return RedirectToAction("Index");
        }

        // Xoá sản phẩm khỏi giỏ
        // /Cart/Remove/5
        public ActionResult Remove(int id)
        {
            var cart = GetCart();
            var line = cart.FirstOrDefault(x => x.ProductId == id);
            if (line != null)
            {
                cart.Remove(line);
            }
            return RedirectToAction("Index");
        }

        // Trang checkout (chưa build)
        public ActionResult Checkout()
        {
            return Content("Trang thanh toán (đang xây dựng)");
        }

        // (Chuẩn bị cho bước sau)
        // /Cart/AddToCart/5
        // Sau này gọi từ product detail
        public ActionResult AddToCart(int id)
        {
            // TODO: sau này bạn lấy sản phẩm thật từ DB
            // Hiện tại ví dụ tạm: nếu có rồi thì +1, nếu chưa có thì thêm mới

            var cart = GetCart();

            var line = cart.FirstOrDefault(x => x.ProductId == id);
            if (line != null)
            {
                line.Quantity += 1;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = id,
                    Name = "Sản phẩm demo " + id,
                    ImagePath = null,
                    Price = 5000000m,
                    Quantity = 1
                });
            }

            return RedirectToAction("Index");
        }
    }
}
