using DemoWeb.Models;
using System;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DemoWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Đăng ký Area, Filter, Route, Bundle
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Không khởi tạo database mặc định
            Database.SetInitializer<AppDbContext>(null);

            // Start cập nhật giá vàng tự động mỗi 5 phút
            DemoWeb.Services.GoldUpdater.Start();
        }
    }
}
