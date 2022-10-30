using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u21454249_HW06.Models
{
    public class Orders
    {
        public int Orderid { get; set; }
        public product Product { get; set; }
        public decimal Price { get; set; }
        public DateTime OrderDate { get; set; }
        public string Category { get; set; }
        public decimal Total { get; set; }
        public int Quantity { get; set; }
       
    }
}