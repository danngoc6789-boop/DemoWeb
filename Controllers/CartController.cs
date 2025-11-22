using DemoWeb.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;

namespace DemoWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext db = new AppDbContext();

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
        public ActionResult Plus(int id,string size)
        {
            var cart = GetCart();
            var line = cart.FirstOrDefault(x => x.ProductId == id && x.Size == size);
            if (line != null)
            {
                line.Quantity += 1;
            }
            return RedirectToAction("Index");
        }

        // Giảm số lượng 1 sản phẩm (tối thiểu 1)
        // /Cart/Minus/5
        public ActionResult Minus(int id,string size)
        {
            var cart = GetCart();
            var line = cart.FirstOrDefault(x => x.ProductId == id && x.Size == size);
            if (line != null)
            {
                line.Quantity = Math.Max(1, line.Quantity - 1);
            }
            return RedirectToAction("Index");
        }

        // Xoá sản phẩm khỏi giỏ
        // /Cart/Remove/5
        public ActionResult Remove(int id, string size)
        {
            var cart = GetCart();
            var line = cart.FirstOrDefault(x => x.ProductId == id && x.Size == size);
            if (line != null)
            {
                cart.Remove(line);
            }
            return RedirectToAction("Index");
        }


        // (Chuẩn bị cho bước sau)
        // /Cart/AddToCart/5
        // Sau này gọi từ product detail
        [HttpPost]
        public ActionResult AddToCart(int id, int quantity, string size)
        {
            var cart = GetCart();
            // Validate quantity
            if (quantity <= 0) quantity = 1;
            // Lấy sản phẩm thật từ database
            var product = db.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return HttpNotFound(); // Nếu không tìm thấy thì báo lỗi
            }
            
            // Kiểm tra xem sản phẩm đã có trong giỏ chưa
            var line = cart.FirstOrDefault(x => x.ProductId == id && x.Size == size);
            if (line != null)
            {
                line.Quantity += quantity;
            }
            else
            {
                // Thêm sản phẩm mới với dữ liệu từ DB
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    ImagePath = product.MainImage, // đường dẫn hình trong CSDL
                    Price = product.Price,
                    Quantity = quantity,
                    Size = size 
                });
            }
            TempData["SuccessMessage"] = $"Đã thêm {product.Name}" +
               (!string.IsNullOrEmpty(size) ? $" (Size: {size})" : "") +
               " vào giỏ hàng!";
            return RedirectToAction("Index");
        }
        // Trang hiển thị form thanh toán
        [HttpGet]
        public ActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Checkout(string CustomerName, string Address, string Phone, string Email,string size)
        {
            var cart = GetCart();
            if (cart == null || !cart.Any())
            {
                ViewBag.Error = "Giỏ hàng của bạn đang trống.";
                return View();
            }

            // Tính tổng tiền
            decimal totalAmount = cart.Sum(x => x.ThanhTien);

            // Tạo mã đơn tự động: ORD + yyyyMMdd + 4 số Id
            string orderCode = "ORD" + DateTime.Now.ToString("yyyyMMddHHmmss");

            // Tạo đơn hàng
            var order = new Order
            {
                Username = User.Identity.Name,
                CustomerName = CustomerName,
                Address = Address,
                Phone = Phone,
                Email = Email,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                OrderCode = orderCode,
                Status = "Đang xử lý"
            };

            db.Orders.Add(order);
            db.SaveChanges(); // Lưu để lấy Id

            // Lưu chi tiết sản phẩm
            // Lưu chi tiết sản phẩm
            foreach (var item in cart)
            {
                var detail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price,
                    Size = item.Size
                };
                db.OrderDetails.Add(detail);
            }

            db.SaveChanges();

            // Xóa giỏ hàng
            Session["Cart"] = null;

            ViewBag.OrderCode = orderCode;
            return View("Success"); // hiển thị trang thành công
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
