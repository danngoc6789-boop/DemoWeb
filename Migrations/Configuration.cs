namespace DemoWeb.Migrations
{
    using DemoWeb.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DemoWeb.Models.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DemoWeb.Models.AppDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<DemoWeb.Models.ApplicationUser>(new UserStore<DemoWeb.Models.ApplicationUser>(context));
            // Tạo Roles
             if (!roleManager.RoleExists("Admin")) { roleManager.Create(new IdentityRole("Admin")); }
            
            if (!roleManager.RoleExists("Customer")) { roleManager.Create(new IdentityRole("Customer")); }
            // Tạo Admin
            if (userManager.FindByEmail("admin@DTJ.com") == null)
            { var admin = new DemoWeb.Models.ApplicationUser 
               { 
                UserName = "admin@DTJ.com",
                Email = "admin@DTJ.com", 
                EmailConfirmed = true,
                FullName = "Quản trị viên", 
                Address = "Hồ Chí Minh", 
                PhoneNumber = "0969934729", 
                CreatedAt = DateTime.Now 
            };
                var result = userManager.Create(admin, "Admin@123");
                if (result.Succeeded)
                { userManager.AddToRole(admin.Id, "Admin");
                } 
            } 
            // Tạo Customer
               if (userManager.FindByEmail("customer@example.com") == null)
               { var customer = new DemoWeb.Models.ApplicationUser 
                  { UserName = "customer@example.com",
                   Email = "customer@example.com",
                   EmailConfirmed = true, 
                   FullName = "Khách hàng mẫu",
                   Address = "Hà Nội",
                   PhoneNumber = "0987654321",
                   CreatedAt = DateTime.Now 
               }; 
                  var result = userManager.Create(customer, "Customer@123");
                    if (result.Succeeded) { userManager.AddToRole(customer.Id, "Customer");
                    }
               } 
            base.Seed(context);

        }
    }
    
}
