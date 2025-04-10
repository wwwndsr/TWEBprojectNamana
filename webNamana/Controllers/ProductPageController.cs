using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using webNamana.Models;

namespace webNamana.Controllers
{
    public class ProductPageController : Controller
    {
        // GET: ProductPage
        public ActionResult ProductPage()
        {
            return View();
        }
    }
}