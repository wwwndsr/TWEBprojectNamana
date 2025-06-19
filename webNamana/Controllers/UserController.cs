using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using webNamana.BusinessLogic;
using webNamana.BusinessLogic.BLogic;
using webNamana.BusinessLogic.Interfaces;
using webNamana.Domain.Entities.User;
using webNamana.Filters;
using webNamana.Helpers;
using webNamana.Models;

namespace webNamana.Controllers
{
    [UserOnly]
    public class UserController : Controller
    {
        private readonly IUserBL _userService;

        public UserController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _userService = bl.GetUserService();
        }

        public ActionResult UserPage()
        {
            var cookie = Request.Cookies["X-KEY"];
            if (cookie == null)
                return RedirectToAction("Login", "Account");

            string email;
            try
            {
                email = CookieGenerator.Validate(cookie.Value);
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _userService.GetUserByEmail(email);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var model = new UserDashboard
            {
                Username = user.Username,
                Email = user.Email,
                Level = user.Level,
                CartItems = new List<string>()
            };

            return View("UserPage", model);
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            var cookie = Request.Cookies["X-KEY"];
            if (cookie == null) return RedirectToAction("Login", "Account");

            string email;
            try
            {
                email = CookieGenerator.Validate(cookie.Value);
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _userService.GetUserByEmail(email);
            if (user == null) return RedirectToAction("Login", "Account");

            var model = new EditProfileViewModel
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Некорректные данные";
                TempData["AlertType"] = "warning";
                return View(model);
            }

            var user = _userService.GetUserByUsername(model.Username);
            if (user == null)
            {
                TempData["Message"] = "Пользователь не найден";
                TempData["AlertType"] = "danger";
                return RedirectToAction("Login", "Account");
            }

            user.Email = model.Email;
            var success = _userService.UpdateUserProfile(user.Username, user);

            if (!success)
            {
                TempData["Message"] = "Ошибка при обновлении профиля";
                TempData["AlertType"] = "danger";
                return View(model);
            }

            TempData["Message"] = "Профиль обновлён";
            TempData["AlertType"] = "success";
            return RedirectToAction("UserPage");
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cookie = Request.Cookies["X-KEY"];
            if (cookie == null)
                return RedirectToAction("Login", "Account");

            string email;
            try
            {
                email = CookieGenerator.Validate(cookie.Value);
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _userService.GetUserByEmail(email);
            if (user == null)
                return RedirectToAction("Login", "Account");

            if (!_userService.ValidateUserCredentials(user.Username, model.CurrentPassword))
            {
                TempData["Message"] = "Текущий пароль неверен";
                TempData["AlertType"] = "danger";
                return View(model);
            }

            var success = _userService.ChangePassword(user.Username, model.CurrentPassword, model.NewPassword);
            if (!success)
            {
                TempData["Message"] = "Ошибка при смене пароля";
                TempData["AlertType"] = "danger";
                return View(model);
            }

            TempData["Message"] = "Пароль успешно изменён";
            TempData["AlertType"] = "success";
            return RedirectToAction("UserPage");
        }
    }
}
