using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace webNamana.Controllers
{
    public class ErrorsController : Controller
    {
        // GET: /Errors/NotFound
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }

        // GET: /Errors/AccessDenied
        public ActionResult AccessDenied()
        {
            Response.StatusCode = 403;
            return View("AccessDenied");
        }

        // GET: /Errors/Error (общее сообщение об ошибке, например 500)
        public ActionResult Error()
        {
            Response.StatusCode = 500;
            return View("Error");
        }
    }
}