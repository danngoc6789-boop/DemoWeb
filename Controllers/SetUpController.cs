using DemoWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DemoWeb.Controllers
{
    public class SetupController : Controller
    {
        public async Task<ActionResult> CreateUsers()
        {
            var db = new AppDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            string html = "<h2>Kết quả tạo Users và Roles:</h2><ul>";

            try
            {
                // Tạo Roles
                if (!roleManager.RoleExists("Admin"))
                {
                    roleManager.Create(new IdentityRole("Admin"));
                    html += "<li style='color:green'>✅ Đã tạo Role: Admin</li>";
                }
                else
                {
                    html += "<li>ℹ️ Role Admin đã tồn tại</li>";
                }

                if (!roleManager.RoleExists("Customer"))
                {
                    roleManager.Create(new IdentityRole("Customer"));
                    html += "<li style='color:green'>✅ Đã tạo Role: Customer</li>";
                }
                else
                {
                    html += "<li>ℹ️ Role Customer đã tồn tại</li>";
                }

                // ===== THAY ĐỔI EMAIL ADMIN =====
                var adminEmail = "admin@DTJ.com"; // SỬA LẠI
                var existingAdmin = userManager.FindByEmail(adminEmail);

                if (existingAdmin == null)
                {
                    var admin = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true,
                        FullName = "Quản trị viên",
                        Address = "Hồ Chí Minh",
                        PhoneNumber = "0969934729",
                        CreatedAt = DateTime.Now
                    };

                    var result = await userManager.CreateAsync(admin, "Admin@123");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin.Id, "Admin");
                        html += "<li style='color:green'>✅ Đã tạo User: admin@DTJ.com</li>";
                        html += "<li style='color:green'>✅ Đã gán Role Admin</li>";
                    }
                    else
                    {
                        html += "<li style='color:red'>❌ Lỗi: " + string.Join(", ", result.Errors) + "</li>";
                    }
                }
                else
                {
                    html += "<li>ℹ️ User admin đã tồn tại</li>";
                    var roles = await userManager.GetRolesAsync(existingAdmin.Id);
                    if (!roles.Contains("Admin"))
                    {
                        await userManager.AddToRoleAsync(existingAdmin.Id, "Admin");
                        html += "<li style='color:green'>✅ Đã gán Role Admin</li>";
                    }
                }

                // Tạo Customer
                var customerEmail = "customer@example.com";
                var existingCustomer = userManager.FindByEmail(customerEmail);

                if (existingCustomer == null)
                {
                    var customer = new ApplicationUser
                    {
                        UserName = customerEmail,
                        Email = customerEmail,
                        EmailConfirmed = true,
                        FullName = "Khách hàng mẫu",
                        Address = "Hà Nội",
                        PhoneNumber = "0987654321",
                        CreatedAt = DateTime.Now
                    };

                    var result = await userManager.CreateAsync(customer, "Customer@123");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(customer.Id, "Customer");
                        html += "<li style='color:green'>✅ Đã tạo User: customer@example.com</li>";
                        html += "<li style='color:green'>✅ Đã gán Role Customer</li>";
                    }
                }
                else
                {
                    html += "<li>ℹ️ User customer đã tồn tại</li>";
                }

                html += "</ul><hr/>";
                html += "<h3>Tài khoản đăng nhập:</h3>";
                html += "<p><strong>Admin:</strong> admin@DTJ.com / Admin@123</p>";
                html += "<p><strong>Customer:</strong> customer@example.com / Customer@123</p>";
                html += "<hr/>";
                html += "<p><a href='/Account/Login' style='padding:10px 20px; background:#28a745; color:white; text-decoration:none; border-radius:5px;'>Đi tới trang Đăng nhập</a></p>";
            }
            catch (Exception ex)
            {
                html += $"<li style='color:red'>❌ LỖI: {ex.Message}</li>";
            }

            return Content(html, "text/html");
        }
    }
}
