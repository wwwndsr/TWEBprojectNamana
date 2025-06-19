using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace webNamana.Models
{
    public class ProductCreateViewModel
    {
        [Required]
        [Display(Name = "Product name")]
        public string ProductName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Range(0.01, 99999)]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Product image")]
        public HttpPostedFileBase ProductImageFile { get; set; }
    }
}