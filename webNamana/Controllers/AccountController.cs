using System;
using System.Web;
using System.Web.Mvc;
using webNamana.BusinessLogic;
using webNamana.BusinessLogic.Interfaces;
using webNamana.Domain.Entities.User;
using webNamana.Domain.Enums;
using webNamana.Helpers;
using webNamana.Models;

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
            return View(new LoginViewModel());
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!_user.ValidateUserCredentialsByEmail(model.Email, model.Password))
            {
                ModelState.AddModelError("", "Неверный email или пароль.");
                return View(model);
            }

            var user = _user.GetUserByEmail(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Пользователь не найден.");
                return View(model);
            }

            _user.UpdateUserLoginData(user.Email, Request.ServerVariables["REMOTE_ADDR"]);

            SetUserCookie(user.Username);
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/SignUp
        public ActionResult SignUp()
        {
            return View(new SignUpViewModel());
        }

        // POST: /Account/SignUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (_user.GetUserByEmail(model.Email) != null)
            {
                ModelState.AddModelError("", "Email уже зарегистрирован.");
                return View(model);
            }

            if (_user.GetUserByUsername(model.Username) != null)
            {
                ModelState.AddModelError("", "Имя пользователя занято.");
                return View(model);
            }

            var newUser = new UDbTable
            {
                Username = model.Username,
                Email = model.Email,
                Password = LoginHelper.HashGen(model.Password),
                RegisterTime = DateTime.Now,
                LastLogin = DateTime.Now,
                Level = URole.User,
                LasIp = Request.ServerVariables["REMOTE_ADDR"]
            };

            if (!_user.CreateUser(newUser))
            {
                ModelState.AddModelError("", "Ошибка регистрации.");
                return View(model);
            }

            SetUserCookie(newUser.Username);
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Logout
        public ActionResult Logout()
        {
            RemoveUserCookie();
            return RedirectToAction("Login");
        }

        // GET: /Account/GoToProfile
        public ActionResult GoToProfile()
        {
            var username = GetUsernameFromCookie();
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Account");

            var user = _user.GetUserByUsername(username);
            if (user == null)
                return RedirectToAction("Login", "Account");

            return user.Level == URole.Admin
                ? RedirectToAction("AdminPage", "Admin")
                : RedirectToAction("UserPage", "User");
        }

        // === Private helpers ===

        private void SetUserCookie(string username)
        {
            var cookie = new HttpCookie(CookieName, username)
            {
                Expires = DateTime.Now.AddDays(7),
                HttpOnly = true,
                Secure = Request.IsSecureConnection,
                Path = "/"
            };
            Response.Cookies.Add(cookie);
        }

        private void RemoveUserCookie()
        {
            var cookie = new HttpCookie(CookieName)
            {
                Expires = DateTime.Now.AddDays(-1),
                Path = "/"
            };
            Response.Cookies.Add(cookie);
        }

        private string GetUsernameFromCookie()
        {
            var cookie = Request.Cookies[CookieName];
            return cookie?.Value;
        }
    }
}
