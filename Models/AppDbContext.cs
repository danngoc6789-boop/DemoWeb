using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace DemoWeb.Models
{
    public class AppDbContext : DbContext
    {
        
        public AppDbContext() : base("DefaultConnection")
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }


        public DbSet<GoldPrice> GoldPrices { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Bỏ qua các property NotMapped
            modelBuilder.Entity<Product>().Ignore(p => p.ImageList);
            modelBuilder.Entity<Product>().Ignore(p => p.MainImage);
        }

    }
}