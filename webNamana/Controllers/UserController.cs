using System.Web.Mvc;
using webNamana.BusinessLogic.Interfaces;
using webNamana.Domain.Entities.User;
using webNamana.Models;
using webNamana.Filters;
using webNamana.Helpers;

namespace webNamana.Controllers
{
    [UserOnly]
    public class UserController : Controller
    {
        private readonly IUserBL _user;

        public UserController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _user = bl.GetUserBl();
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

        [HttpPost]
        public ActionResult EditProfile(UserMinimal model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Некорректные данные.";
                return View("Profile", model);
            }

            // Вызов бизнес-логики
            var result = _user.EditUserProfile(model);

            if (result.Status)
            {
                // Обновляем сессию, если нужно
                SessionHelper.User = new UserMinimal
                {
                    Id = model.Id,
                    Username = model.Username,
                    Email = model.Email
                };

                TempData["Message"] = "Данные обновлены.";
            }
            else
            {
                TempData["Message"] = result.StatusMsg ?? "Ошибка при обновлении данных.";
            }

            return RedirectToAction("Profile");
        }

        // Страница редактирования профиля (форма)
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
    }
}
