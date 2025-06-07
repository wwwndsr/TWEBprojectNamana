using System;
using System.Web.Mvc;
using webNamana.Models;
using webNamana.Domain.Entities.User;
using webNamana.BusinessLogic.DBModel;
using System.Security.Cryptography;
using System.Text;
using webNamana.Domain.Enums;

namespace webNamana.Controllers
{
    public class SignUpController : Controller
    {
        // GET: SignUp
        public ActionResult SignUp()
        {
            return View(new SignUp());
        }

        // POST: SignUp
        [HttpPost]
        public ActionResult SignUp(SignUp model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new UserContext())
                {
                    // Хэширование пароля
                    var passwordHash = HashPassword(model.Password);

                    var user = new UDbTable
                    {
                        Username = model.Username,
                        Email = model.Email,
                        Password = passwordHash,  // Сохраняем захэшированный пароль
                        LastLogin = DateTime.Now,
                        LastIp = Request.UserHostAddress,
                        Level = URole.User
                    };

                    db.Users.Add(user);
                    db.SaveChanges();
                }

                return RedirectToAction("Index", "Home");
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
    }
}
