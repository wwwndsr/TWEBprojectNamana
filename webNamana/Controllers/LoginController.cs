using System;
using System.Web.Mvc;
using webNamana.Models;
using webNamana.Domain.Entities.User;
using webNamana.BusinessLogic.DBModel;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

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
                using (var db = new UserContext())
                {
                    // Хэшируем введенный пароль
                    var passwordHash = HashPassword(model.Password);

                    // Ищем пользователя с таким email и хэшированным паролем
                    var user = db.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == passwordHash);
                    if (user != null)
                    {
                        // Сохраняем email пользователя в сессии
                        Session["UserEmail"] = user.Email;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Неверный email или пароль");
                    }
                }
            }

            return View(model);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }

        public ActionResult Logout()
        {
            // Очищаем сессию
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
