using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using webNamana.BusinessLogic.Interfaces;

namespace webNamana.Controllers
{
    public class LoginController : Controller
    {
       /*
        private readonly ISession _session;
        public LoginController()
        {
            var bl = new webNamana.BusinessLogic.BusinessLogic();
            _session = bl.GetSessionBL();
        } */

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        /*
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Login(UserLogin login)
        {
         if (ModelState.IsValid)
            {
                ULoginData data = new ULoginData
                {
                    Credential = login.Credential,
                    Password = login.Password,
                    LoginIp = Request.UserHostAddress,
                    LoginDateTime = DateTime.Now
                };
                var userLogin = _session.UserLogin(data);
                if (userLogin.Status)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                     ModelState.AddModelError("", userLogin.StatusMsg);
                    return View();
                }

            }
         return View();
        }*/
    }
}