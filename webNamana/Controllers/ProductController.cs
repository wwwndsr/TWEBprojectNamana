using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using webNamana.Models;

namespace webNamana.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product

        public ActionResult Product(int id = 1)
        {
            var product = new Product
            {
                ProductId = id,
                ProductName = "Меч Арканы",
                Description = "Описание товара",
                Price = 999.99m,
                ProductImage = "image.png"
            };

            return View(product);
        }

    }
}
