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
        private readonly IUserService _userService;

        public UserController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _userService = bl.GetUserService();
        }

        //[Authorize]
        public ActionResult UserPage()
        {
            var cookie = Request.Cookies["X-KEY"];
            if (cookie == null)
                return RedirectToAction("Login", "Account");

            var username = cookie.Value;
            var user = _userService.GetUserByUsername(username);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var model = new UserDashboard
            {
                Username = user.Username,
                Email = user.Email,
                Level = user.Level,

                // Просто пустой список, без фейковых данных
                CartItems = new List<string>(),
            };

            return View("UserPage", model);
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            var cookie = Request.Cookies["X-KEY"];
            if (cookie == null) return RedirectToAction("Login", "Account");

            var username = cookie.Value;
            var user = _userService.GetUserByUsername(username);
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

            var username = cookie.Value;

            // Проверка текущего пароля
            if (!_userService.ValidateUserCredentials(username, model.CurrentPassword))
            {
                TempData["Message"] = "Текущий пароль неверен";
                TempData["AlertType"] = "danger";
                return View(model);
            }

            // Смена пароля
            var success = _userService.ChangePassword(username, model.CurrentPassword, model.NewPassword);
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

        /* [HttpGet]
         public ActionResult ViewOrders()
         {
             var cookie = Request.Cookies["X-KEY"];
             if (cookie == null) return RedirectToAction("Login", "Account");

             var username = cookie.Value;
             var orders = _userService.GetOrdersByUsername(username);

             return View(orders);
         }*/
    }
}
