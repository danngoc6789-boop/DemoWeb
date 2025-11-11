using DemoWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.EnterpriseServices;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DemoWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Đăng ký Area, Filter, Route, Bundle - CHỈ GỌI 1 LẦN
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Không khởi tạo database mặc định
            Database.SetInitializer<AppDbContext>(null);

            // Tạo user + role admin nếu chưa có
            InitializeAdmin();

            // Start cập nhật giá vàng tự động mỗi 5 phút
            try
            {
                DemoWeb.Services.GoldUpdater.Start();
            }
            catch
            {
                // Bỏ qua nếu GoldUpdater chưa được tạo
            }
        }

        private void InitializeAdmin()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                    // 1. Tạo role Admin nếu chưa có
                    if (!roleManager.RoleExists("Admin"))
                    {
                        var role = new IdentityRole("Admin");
                        roleManager.Create(role);
                    }

                    // 2. Tạo role Customer nếu chưa có
                    if (!roleManager.RoleExists("Customer"))
                    {
                        var role = new IdentityRole("Customer");
                        roleManager.Create(role);
                    }

                    // 3. Tạo tài khoản admin nếu chưa có
                    var adminUser = userManager.FindByEmail("DTJAdmin@gmail.com");
                    if (adminUser == null)
                    {
                        var admin = new ApplicationUser
                        {
                            UserName = "AdminDTJ.com",
                            Email = "DTJAdmin@gmail.com",
                            FullName = "Quản trị viên"
                        };

                        var result = userManager.Create(admin, "Admin@123");
                        if (result.Succeeded)
                        {
                            userManager.AddToRole(admin.Id, "Admin");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                System.Diagnostics.Debug.WriteLine($"Lỗi khởi tạo Admin: {ex.Message}");
            }
        }
    }
}
