using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoWeb.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }      // ID sản phẩm
        public string Name { get; set; }        // Tên sản phẩm
        public string ImagePath { get; set; }   // Link hình
        public decimal Price { get; set; }      // Giá 1 cái
        public int Quantity { get; set; }       // Số lượng mua

        public decimal ThanhTien
        {
            get { return Price * Quantity; }
        }
    }
}