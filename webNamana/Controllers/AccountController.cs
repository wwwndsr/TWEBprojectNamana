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
using webNamana.Models;
using webNamana.BusinessLogic.BLogic;

namespace webNamana.Controllers
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
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.LasIp = Request.UserHostAddress;

            // Проверка логина
            if (!_user.ValidateUserCredentialsByEmail(model.Email, model.Password))
            {
                ModelState.AddModelError("", "Неверный email или пароль.");
                return View(model);
            }

            // Получаем пользователя
            var user = _user.GetUserByEmail(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Пользователь не найден.");
                return View(model);
            }

            // Обновляем время входа и IP
            _user.UpdateUserLoginData(model.Email, model.LasIp);

            // Сохраняем пользователя в сессию
            SessionHelper.SetUserSession(user.Email);
            SessionHelper.User = new UserMinimal
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Level = user.Level
            };

            // Создаём куку через SessionBL
            var sessionBl = new SessionBL();
            var cookie = sessionBl.GenCookie(user.Email);
            Response.Cookies.Add(cookie);

            return RedirectToAction("Index", "Home");
        }




        // GET: /Account/SignUp
        public ActionResult SignUp()
        {
            return View();
        }

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
            SessionHelper.SetUserSession(model.Email);
            SessionHelper.User = new UserMinimal
            {
                Id = model.Id,
                Username = model.Username,
                Email = model.Email,
                Level = model.Level
            };

            var sessionBl = new SessionBL();
            var cookie = sessionBl.GenCookie(model.Email);
            Response.Cookies.Add(cookie);


            // Перенаправляем на главную страницу
            return RedirectToAction("Index", "Home");
        }



        // GET: /Account/Logout
        public ActionResult Logout()
        {
            var cookie = Request.Cookies[CookieName];
            if (cookie != null)
            {
                var sessionBl = new SessionBL();
                sessionBl.SignOut(cookie.Value);

                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }

            SessionHelper.ClearSession();
            return RedirectToAction("Login");
        }


        private UDbTable GetCurrentUser()
        {
            var cookie = Request.Cookies[CookieName];
            if (cookie == null)
                return null;

            var email = cookie.Value;
            return _user.GetUserByEmail(email);

        }
        public ActionResult GoToProfile()
        {
            if (!SessionHelper.IsUserLoggedIn())
                return RedirectToAction("Login");

            var user = SessionHelper.User;
            return user.Level == URole.Admin
                ? RedirectToAction("AdminPage", "Admin")
                : RedirectToAction("UserPage", "User");
        }

    }
}
