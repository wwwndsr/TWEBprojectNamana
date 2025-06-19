using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace webNamana.Models
{
    public class ProductEditViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Product name")]
        public string ProductName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Range(0.01, 99999)]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Existing image path")]
        public string ExistingImage { get; set; }

        [Display(Name = "Upload new image")]
        public HttpPostedFileBase ProductImageFile { get; set; }
    }
}