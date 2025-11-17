using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DemoWeb.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
       
        public AppDbContext() : base("DefaultConnection")
        {
            // Tắt auto-migration
            Database.SetInitializer<AppDbContext>(null);
        }
        public static AppDbContext Create()
        {
            return new AppDbContext();
        }
        ///tất cả bảng
        public DbSet<Product> Products { get; set; }
        
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<GoldPrice> GoldPrices { get; set; }

        public DbSet<SupportRequest> SupportRequests { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Bỏ qua các property NotMapped
            modelBuilder.Entity<Product>().Ignore(p => p.ImageList);
            modelBuilder.Entity<Product>().Ignore(p => p.MainImage);


        }
      
    }
}