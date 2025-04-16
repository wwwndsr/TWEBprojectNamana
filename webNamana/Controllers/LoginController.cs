using System;
using System.Collections.Generic;
using System.Web;
using webNamana.Models;

using System.Linq;
using System.Web.Mvc;


namespace webNamana.Controllers
{

        public class LoginController : Controller
        {

            // GET: Login
            public ActionResult Login()
            {
                return View();
            }

            [HttpPost]
            public ActionResult Login(Login model)
            {
            if (ModelState.IsValid)
            {
                // Пример проверки учетных данных (замените на реальную проверку через базу данных)
                if (AuthenticateUser(model.Email, model.Password))
                {
                    // Сохраняем email пользователя в сессии
                    Session["UserEmail"] = model.Email;

                    // Перенаправляем на главную страницу
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Добавляем сообщение об ошибке
                    ModelState.AddModelError("", "Неправильный email или пароль.");
                }
            }
                return View(model);
            }

        private bool AuthenticateUser(string email, string password)
        {
            // Пример проверки (замените на реальную логику проверки через базу данных)
            return email == "test@example.com" && password == "123456";
        }

        public ActionResult Logout()
        {
            // Очищаем сессию
            Session.Clear();
            return RedirectToAction("Login");
        }
    }

 }