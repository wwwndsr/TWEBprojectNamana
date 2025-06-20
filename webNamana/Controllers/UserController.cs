using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webNamana.BusinessLogic;
using webNamana.BusinessLogic.Interfaces;
using webNamana.Filters;
using webNamana.Helpers;
using webNamana.Models;
using webNamana.BusinessLogic.DBModel;

namespace webNamana.Controllers
{
    [UserOnly]
    public class UserController : Controller
    {
        private readonly IUserBL _userService;
        private readonly ICartBL _cartService;

        public UserController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _userService = bl.GetUserBL();
            _cartService = bl.GetCartBL(); // Добавлено для работы с корзиной
        }

        private string GetSessionId()
        {
            return Session.SessionID ?? Guid.NewGuid().ToString();
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

            string sessionId = GetSessionId();
            var cartEntities = _cartService.GetCartItems(sessionId);
            var cartItems = cartEntities.Select(c => $"{c.ProductName} x{c.Quantity}").ToList();

            var model = new UserDashboard
            {
                Username = user.Username,
                Email = user.Email,
                Level = user.Level,
                CartItems = cartItems,
                Trainings = GetUserTrainings(user.Username)
            };

            return View("UserPage", model);
        }

        private List<TrainingInfoViewModel> GetUserTrainings(string username)
        {
            using (var db = new TrainingContext())
            {
                return db.Registrations
                    .Where(r => r.Username == username)
                    .Select(r => new TrainingInfoViewModel
                    {
                        TrainingType = r.TrainingType,
                        RegistrationDate = r.RegistrationDate,
                        TrainingTime = r.TrainingTime,
                        IsConfirmed = r.IsConfirmed
                    })
                    .ToList();
            }
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

        public ActionResult Dashboard()
        {
            var email = SessionHelper.GetCurrentUsername();
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Account");

            List<TrainingInfoViewModel> trainings = new List<TrainingInfoViewModel>();

            using (var db = new TrainingContext())
            {
                var registrations = db.Registrations.ToList(); // без Where

                foreach (var r in registrations)
                {
                    if (r.Username == email)
                    {
                        trainings.Add(new TrainingInfoViewModel
                        {
                            TrainingType = r.TrainingType,
                            RegistrationDate = r.RegistrationDate,
                            TrainingTime = r.TrainingTime,
                            IsConfirmed = r.IsConfirmed
                        });
                    }
                }
            }

            var model = new UserDashboard
            {
                Username = SessionHelper.User?.Username,
                Email = SessionHelper.User?.Email,
                CartItems = new List<string>(), // если используешь корзину — заполни
                Trainings = trainings
            };

            return View("UserPage", model);
        }

        public ActionResult UserRegistrations()
        {
            if (!SessionHelper.IsUserLoggedIn())
                return RedirectToAction("Login", "Account");

            string username = SessionHelper.User?.Username;
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Account");

            List<TrainingRegisterViewModel> registrations;
            using (var db = new TrainingContext())
            {
                registrations = db.Registrations
                    .Where(r => r.Username == username)
                    .Select(r => new TrainingRegisterViewModel
                    {
                        TrainingType = r.TrainingType,
                        RegistrationDate = r.RegistrationDate,
                        TrainingTime = r.TrainingTime,
                        IsConfirmed = r.IsConfirmed,
                        CreatedAt = r.CreatedAt
                    })
                    .ToList();
            }

            return View(registrations); // передать в представление список
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
