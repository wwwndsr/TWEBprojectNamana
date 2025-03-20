using System;
using System.Collections.Generic;
using System.Web;
using webNamana.Models;

using System.Linq;
using System.Web.Mvc;



namespace webNamana.Controllers
{
    public class LoginController : Controller
    { 

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Login model)
        {
            if (ModelState.IsValid)
            {
                if (model.Email == "test@example.com" && model.Password == "123456")
                {
                    ViewBag.Message = "Успешный вход!";
                }
                else
                {
                    ViewBag.Message = "Ошибка: неправильный email или пароль.";
                }
            }

            return View(model);
        }
    }
}