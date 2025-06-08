using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webNamana.Models
{
	public class ProductViewModel
	{
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ProductImage { get; set; }
    }
}