using DemoWeb.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DemoWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext db = new AppDbContext();

        // GET: Product/Index
        public ActionResult Index(int? page, string loai, string q)
        {
            int pageSize = 12; // mỗi trang 12 sản phẩm
            int pageNumber = (page ?? 1);

            var products = db.Products.AsQueryable();

            // Bộ lọc loại vàng
            if (!string.IsNullOrEmpty(loai))
            {
                products = products.Where(p => p.Type == loai);
            }

            // Tìm kiếm
            if (!string.IsNullOrEmpty(q))
            {
                products = products.Where(p => p.Name.Contains(q));
            }

            ViewBag.CurrentLoai = loai;
            ViewBag.CurrentQ = q;

            // QUAN TRỌNG: phải chuyển sang ToPagedList
            return View(products
                .OrderByDescending(p => p.Id)
                .ToPagedList(pageNumber, pageSize));
        }
        
        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, HttpPostedFileBase ImageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Xử lý upload ảnh
                    if (ImageFile != null && ImageFile.ContentLength > 0)
                    {
                        // Validate file type
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                        var extension = Path.GetExtension(ImageFile.FileName).ToLower();

                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("ImageFile", "Chỉ chấp nhận file ảnh (.jpg, .png, .gif, .webp)");
                            return View(product);
                        }

                        // Validate file size (5MB)
                        if (ImageFile.ContentLength > 5 * 1024 * 1024)
                        {
                            ModelState.AddModelError("ImageFile", "Kích thước file không được vượt quá 5MB");
                            return View(product);
                        }

                        // Tạo tên file unique
                        string fileName = Guid.NewGuid().ToString() + extension;
                        string uploadPath = Server.MapPath("~/Content/Images/Products");

                        // Tạo thư mục nếu chưa có
                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }

                        // Lưu file
                        string filePath = Path.Combine(uploadPath, fileName);
                        ImageFile.SaveAs(filePath);

                        // Lưu đường dẫn vào database
                        product.Image = "/Content/Images/Products/" + fileName;
                        product.Images = "~/Content/Images/Products/" + fileName;
                    }

                    product.CreatedDate = DateTime.Now;
                    db.Products.Add(product);
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
                    return RedirectToAction("Index");
                }

                return View(product);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                return View(product);
            }
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy mã sản phẩm!";
                return RedirectToAction("Index");
            }

            var product = db.Products.Find(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy sản phẩm!";
                return RedirectToAction("Index");
            }

            return View(product);
        }
        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, HttpPostedFileBase ImageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingProduct = db.Products.Find(product.Id);
                    if (existingProduct == null)
                    {
                        TempData["ErrorMessage"] = "Không tìm thấy sản phẩm!";
                        return RedirectToAction("Index");
                    }

                    // Cập nhật thông tin
                    existingProduct.Name = product.Name;
                    existingProduct.Type = product.Type;
                    existingProduct.Price = product.Price;
                    existingProduct.Description = product.Description;

                    // Xử lý upload ảnh mới (nếu có)
                    if (ImageFile != null && ImageFile.ContentLength > 0)
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                        var extension = Path.GetExtension(ImageFile.FileName).ToLower();

                        if (allowedExtensions.Contains(extension))
                        {
                            // Xóa ảnh cũ
                            if (!string.IsNullOrEmpty(existingProduct.Image))
                            {
                                string oldPath = Server.MapPath(existingProduct.Image);
                                if (System.IO.File.Exists(oldPath))
                                {
                                    System.IO.File.Delete(oldPath);
                                }
                            }

                            // Lưu ảnh mới
                            string fileName = Guid.NewGuid().ToString() + extension;
                            string uploadPath = Server.MapPath("~/Content/Images/Products");

                            if (!Directory.Exists(uploadPath))
                            {
                                Directory.CreateDirectory(uploadPath);
                            }

                            string filePath = Path.Combine(uploadPath, fileName);
                            ImageFile.SaveAs(filePath);
                            existingProduct.Image = "/Content/Images/Products/" + fileName;
                            if (!string.IsNullOrEmpty(existingProduct.Images))
                            {
                                existingProduct.Images += ";~/Content/Images/Products/" + fileName;
                            }
                            else
                            {
                                existingProduct.Images = "~/Content/Images/Products/" + fileName;
                            }
                        }
                    }

                    db.Entry(existingProduct).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                    return RedirectToAction("Index");
                }

                return View(product);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                return View(product);
            }
        }

        
        // GET: Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Product ID is required");
            }

            var product = db.Products.FirstOrDefault(p => p.Id == id.Value);
            if (product == null)
                return HttpNotFound();

            return View(product);
        }
        // GET: Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var product = db.Products.Find(id);
            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                db.Products.Remove(product);
                db.SaveChanges();
            }

            TempData["SuccessMessage"] = "Đã xóa sản phẩm thành công!";
            return RedirectToAction("Index");
        }


        // GET: Product/AddToCart/5
        public ActionResult AddToCart(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Product ID is required");
            }

            var product = db.Products.FirstOrDefault(p => p.Id == id.Value);
            if (product == null)
                return HttpNotFound();

            var cart = Session["Cart"] as List<Product> ?? new List<Product>();
            cart.Add(product);
            Session["Cart"] = cart;

            TempData["Message"] = $"Đã thêm '{product.Name}' vào giỏ hàng!";
            return RedirectToAction("Index");
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