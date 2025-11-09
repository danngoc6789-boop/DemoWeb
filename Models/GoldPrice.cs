using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoWeb.Models
{
    public class GoldPrice
    {
        public int Id { get; set; }             // nếu dùng DB; nếu không dùng, có thể bỏ
        public string ProductName { get; set; } // tên (n_x)
        public string Karat { get; set; }       // k_x
        public string Purity { get; set; }      // h_x (string để giữ 99.9 / 999.9)
        public decimal BuyPrice { get; set; }   // pb_x
        public decimal SellPrice { get; set; }  // ps_x
        public DateTime Timestamp { get; set; } // d_x
        public string Source { get; set; }      // nguồn API
    }
}