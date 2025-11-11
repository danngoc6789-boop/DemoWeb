using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoWeb.Models
{
    [Table("GoldPrices")]
    public class GoldPrice
    {
        [Key]
        public int Id { get; set; }

        [StringLength(500)]
        public string ProductName { get; set; }

        [StringLength(50)]
        public string Karat { get; set; }

        [StringLength(50)]
        public string Purity { get; set; }

        public decimal BuyPrice { get; set; }

        public decimal SellPrice { get; set; }

        public DateTime Timestamp { get; set; }

        [StringLength(500)]
        public string Source { get; set; }

        // Thêm field này để track khi nào data được tạo
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}