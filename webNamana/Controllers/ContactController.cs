﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace webNamana.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Contact()
        {
            return View();
        }
    }
}