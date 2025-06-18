using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using webNamana.BusinessLogic;
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

            var model = new UserMinimal
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Level = user.Level,
                LastLogin = user.LastLogin,
                RegisterTime = user.RegisterTime
            };

            return View("UserPage", model);
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
        [HttpPost]
         public ActionResult UploadAvatar(HttpPostedFileBase avatar)
        {
            var cookie = Request.Cookies["X-KEY"];
            if (cookie == null || avatar == null || avatar.ContentLength == 0)
            {
                TempData["Message"] = "Не удалось загрузить изображение.";
                TempData["AlertType"] = "danger";
                return RedirectToAction("UserPage");
            }

            string username = cookie.Value;
            string fileName = username + ".png"; 
            string path = Server.MapPath("~/Content/Avatars/");

            // Убедись, что папка существует
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fullPath = Path.Combine(path, fileName);

            try
            {
                avatar.SaveAs(fullPath);
                TempData["Message"] = "Аватар успешно обновлён.";
                TempData["AlertType"] = "success";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Ошибка при сохранении файла: " + ex.Message;
                TempData["AlertType"] = "danger";
            }

            return RedirectToAction("UserPage");
        }
    }
}
