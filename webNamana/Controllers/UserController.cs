using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webNamana.Models;
using webNamana.BusinessLogic.DBModel;

namespace webNamana.Controllers
{
    public class UserController : Controller
    {
        // Страница профиля пользователя
        public ActionResult UserPage()
        {
            var userModel = new UserDashboard
            {
                Username = "JohnDoe",
                Email = "johndoe@example.com",
                CartItems = new List<string> { "Item 1", "Item 2", "Item 3" }
            };

            return View(userModel);
        }

        // Страница редактирования профиля
        public ActionResult EditProfile()
        {
            // Здесь можно передать модель, если она нужна для формы
            var userModel = new EditProfileViewModel
            {
                Username = "JohnDoe",
                Email = "johndoe@example.com"
            };

            return View(userModel); // должно быть представление Views/User/EditProfile.cshtml
        }
    }
}





