using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webNamana.Models
{
	public class Cart
	{
        
		public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; } 
    }
}