using System;
using System.Web.Mvc;
using webNamana.BusinessLogic;
using webNamana.BusinessLogic.Interfaces;
using webNamana.Domain.Entities.User;
using webNamana.Filters;
using webNamana.Helpers;

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

        public new ActionResult Profile()
        {
            var user = SessionHelper.User;

            var model = new UserMinimal
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };

            return View(model);
        }

        public ActionResult EditProfile()
        {
            var user = SessionHelper.User;

            var model = new UserMinimal
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EditProfile(UserMinimal model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Некорректные данные.";
                return View("Profile", model);
            }

            var user = _userService.GetUserByUsername(model.Username);
            if (user == null)
            {
                TempData["Message"] = "Пользователь не найден.";
                return RedirectToAction("Profile");
            }

            user.Username = model.Username;
            user.Email = model.Email;

            var updateResult = _userService.UpdateUserProfile(user.Username, user);

            if (updateResult)
            {
                SessionHelper.User = new UserMinimal
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email
                };

                TempData["Message"] = "Данные обновлены.";
            }
            else
            {
                TempData["Message"] = "Ошибка при обновлении профиля.";
            }

            return RedirectToAction("Profile");
        }
    }
}
