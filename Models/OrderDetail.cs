using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DemoWeb.Models
{
   
        public class OrderDetail
        {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public int Quantity { get; set; }
        public string Size { get; set; }


        public decimal UnitPrice { get; set; }

        // Navigation properties
       
        public virtual Order Order { get; set; }
        
        public virtual Product Product { get; set; }
    }
}

