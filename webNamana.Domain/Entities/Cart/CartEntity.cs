using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webNamana.Domain.Entities.Cart
{
    public class CartEntity
    {
        public int ItemId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; } 
        public bool InStock { get; set; }
    }
}
