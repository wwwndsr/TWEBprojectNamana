using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webNamana.Domain.Entities.User;
using webNamana.Helpers;
using webNamana.BusinessLogic.Services;
using webNamana.BusinessLogic.Interfaces;



namespace webNamana.Web.Controllers
{
    public class AccountController : Controller
    {
        private const string CookieName = "X-KEY";

        private readonly IUserService _user;

        public AccountController()
        {
            _user = new UserService();
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

            // Валидация пользователя через сервис
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

        // GET: /Account/Profile
        [Authorize]
        public ActionResult UserProfile()
        {
            var user = GetCurrentUser();
            if (user == null)
                return RedirectToAction("Login");

            return View(user);
        }

        // POST: /Account/Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult EditProfile(UDbTable model)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
                return RedirectToAction("Login");

            // Обновление через сервис
            bool updated = _user.UpdateUserProfile(currentUser.Username, model);
            if (!updated)
            {
                ModelState.AddModelError("", "Ошибка при обновлении профиля.");
                return View(currentUser);
            }

            ViewBag.SuccessMessage = "Профиль успешно обновлен.";
            return View(_user.GetUserByUsername(currentUser.Username));
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
    }
}
