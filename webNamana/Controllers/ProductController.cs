using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace webNamana.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Product()
        {
            return View();
        }
    }
}