using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webNamana.BusinessLogic;
using webNamana.BusinessLogic.Interfaces;
using webNamana.BusinessLogic.Services;
using webNamana.Domain.Entities.User;
using webNamana.Domain.Enums;
using webNamana.Helpers;

namespace webNamana.Web.Controllers
{
    public class AccountController : Controller
    {
        private const string CookieName = "X-KEY";

        private readonly IUserService _user;

        public AccountController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _user = bl.GetUserService();
        }

        // GET: /Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UDbTable model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.LasIp = Request.UserHostAddress;

            if (!_user.ValidateUserCredentials(model.Username, model.Password))
            {
                ModelState.AddModelError("", "Неверный логин или пароль.");
                return View(model);
            }

            var user = _user.GetUserByUsername(model.Username);

            var cookie = new HttpCookie(CookieName, user.Username)
            {
                Expires = DateTime.Now.AddDays(7)
            };
            Response.Cookies.Add(cookie);

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/SignUp
        public ActionResult SignUp()
        {
            return View();
        }

        // POST: /Account/SignUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(UDbTable model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.RegisterTime = DateTime.Now;
            model.LastLogin = DateTime.Now;
            model.Password = LoginHelper.HashGen(model.Password);

            bool created = _user.CreateUser(model);
            if (!created)
            {
                ModelState.AddModelError("", "Ошибка при регистрации пользователя.");
                return View(model);
            }

            var cookie = new HttpCookie(CookieName, model.Username)
            {
                Expires = DateTime.Now.AddDays(7)
            };
            Response.Cookies.Add(cookie);

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Logout
        public ActionResult Logout()
        {
            var cookie = Request.Cookies[CookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                cookie.Path = "/";
                Response.Cookies.Add(cookie);
            }

            return RedirectToAction("Login");
        }

        private UDbTable GetCurrentUser()
        {
            var cookie = Request.Cookies[CookieName];
            if (cookie == null)
                return null;

            var username = cookie.Value;
            return _user.GetUserByUsername(username);
        }
        public ActionResult GoToProfile()
        {
            var cookie = Request.Cookies["X-KEY"];
            if (cookie == null)
                return RedirectToAction("Login", "Account");

            var username = cookie.Value;

            var user = _user.GetUserByUsername(username); // user — это UDbTable

            if (user == null)
                return RedirectToAction("Login", "Account");

            if (user.Level == URole.Admin)
                return RedirectToAction("AdminPage", "Admin");
            else
                return RedirectToAction("UserPage", "User");
        }

    }
}
