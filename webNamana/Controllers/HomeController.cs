﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace webNamana.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}