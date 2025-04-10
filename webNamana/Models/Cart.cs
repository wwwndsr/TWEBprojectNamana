using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace webNamana.Models
{
	public class Cart
	{
		public int ItemId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; } 
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }   
        public decimal TotalPrice => Price * Quantity;

    }
}