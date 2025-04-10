using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using webNamana.Models;
namespace webNamana.Controllers
{
    public class SignUpController : Controller
    {
        // GET: SignUp
        public ActionResult SignUp()
        {
            var model = new SignUp();
            return View(model);
        }

        // POST: SignUp
        [HttpPost]
        public ActionResult SignUp(SignUp model)
        {
            if (ModelState.IsValid)
            {
                // Логика для сохранения пользователя
                return RedirectToAction("Index", "Home"); // Перенаправление после успешной регистрации
            }

            // Если форма не прошла валидацию, возвращаем модель обратно в представление
            return View(model);
        }
    }
}