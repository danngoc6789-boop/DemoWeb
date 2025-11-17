using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DemoWeb.Models
{
    public class SupportRequest
    {
        public int Id { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string Phone { get; set; }

        public string Email { get; set; }

        [Required]
        public string Message { get; set; }

        public string Status { get; set; } = "Chờ xử lý"; // mặc định

        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}