using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webNamana.BusinessLogic;
using webNamana.BusinessLogic.Interfaces;
using webNamana.Domain.Entities.User;
using webNamana.Domain.Enums;
using webNamana.Helpers;
using webNamana.Models;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace webNamana.Controllers
{
    public class AccountController : Controller
    {
        private const string CookieName = "X-KEY";
        private readonly IUserBL _user;

        public AccountController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _user = bl.GetUserBL();
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

            _user.UpdateUserLoginData(model.Email, model.LasIp);

            // Устанавливаем сессию пользователя
            SessionHelper.SetUserSession(user.Email); // теперь email, не username
            SessionHelper.User = new UserMinimal
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Level = user.Level
            };

            // Устанавливаем cookie
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

        // POST: /Account/SignUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var newUser = new UDbTable
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
                RegisterTime = DateTime.Now,
                LastLogin = DateTime.Now,
                Level = URole.User
            };

            bool created = _user.CreateUser(newUser);

            if (!created)
            {
                ModelState.AddModelError("", "Ошибка при регистрации пользователя: возможно, такой email или имя уже заняты.");
                return View(model);
            }

            // Получаем пользователя из базы, чтобы получить правильный Id
            var registeredUser = _user.GetUserByEmail(newUser.Email);
            if (registeredUser == null)
            {
                ModelState.AddModelError("", "Ошибка при подтверждении регистрации.");
                return View(model);
            }

            SessionHelper.SetUserSession(registeredUser.Email);
            SessionHelper.User = new UserMinimal
            {
                Id = registeredUser.Id,
                Username = registeredUser.Username,
                Email = registeredUser.Email,
                Level = registeredUser.Level
            };

            var sessionBl = new SessionBL();
            var cookie = sessionBl.GenCookie(registeredUser.Email);
            Response.Cookies.Add(cookie);


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
            System.Diagnostics.Debug.WriteLine("GoToProfile called");
            System.Diagnostics.Debug.WriteLine($"SessionHelper.IsUserLoggedIn() = {SessionHelper.IsUserLoggedIn()}");
            System.Diagnostics.Debug.WriteLine($"SessionHelper.User = {(SessionHelper.User == null ? "null" : SessionHelper.User.Username)}");

            if (!SessionHelper.IsUserLoggedIn())
                return RedirectToAction("Login");

            var user = SessionHelper.User;
            if (user == null)
                return RedirectToAction("Login");

            return user.Level == URole.Admin
                ? RedirectToAction("AdminPage", "Admin")
                : RedirectToAction("UserPage", "User");
        }


        /* public ActionResult GoToProfile()
         {
             if (!SessionHelper.IsUserLoggedIn())
                 return RedirectToAction("Login");

             var user = SessionHelper.User;
             return user.Level == URole.Admin
                 ? RedirectToAction("AdminPage", "Admin")
                 : RedirectToAction("UserPage", "User");
         }*/
    }
}
