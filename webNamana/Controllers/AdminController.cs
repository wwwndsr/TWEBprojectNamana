using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using webNamana.BusinessLogic.Interfaces;
using webNamana.Domain.Entities.User;
using webNamana.Domain.Enums;
using webNamana.Filters;
using webNamana.Models;


namespace webNamana.Controllers
{
    [AdminOnly]
    public class AdminController : Controller
    {
        private readonly IAdminBL _admin;

        public AdminController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _admin = bl.GetAdminBL();
        }
        public ActionResult AdminPage()
        {
            var cookie = Request.Cookies["X-KEY"];
            if (cookie == null)
                return RedirectToAction("Login", "Account");

            var username = cookie.Value;
            var result = _admin.GetUserByUsername(username);
            if (!result.Status || result.User == null)
                return RedirectToAction("Login", "Account");

            var user = result.User;

            var model = new UDbTable
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Level = user.Level,
                LastLogin = user.LastLogin,
                RegisterTime = user.RegisterTime
            };

            return View("AdminPage", model);
        }



        // ========== USERS ==========

        public ActionResult Clients()
        {
            try
            {
                var result = _admin.GetAllUsers();
                if (!result.Status)
                {
                    TempData["Message"] = result.StatusMsg ?? "Ошибка при получении списка пользователей";
                    TempData["AlertType"] = "danger";
                    return View(new List<UserMinimal>());
                }

                return View(result.Users);
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Ошибка при загрузке пользователей: {ex.Message}";
                TempData["AlertType"] = "danger";
                return View(new List<UserMinimal>());
            }
        }

        [HttpGet]
        public ActionResult EditUser(int id)
        {
            try
            {
                var result = _admin.GetUserById(id);
                if (!result.Status || result.User == null)
                {
                    TempData["Message"] = result.StatusMsg ?? "Пользователь не найден";
                    TempData["AlertType"] = "warning";
                    return RedirectToAction("Clients");
                }

                var user = result.User;

                // Преобразуем доменную модель в модель представления
                var viewModel = new EditProfileViewModel
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Ошибка при загрузке пользователя: {ex.Message}";
                TempData["AlertType"] = "danger";
                return RedirectToAction("Clients");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Данные некорректны";
                TempData["AlertType"] = "warning";
                return View(model);
            }

            try
            {
                // Преобразуем модель представления обратно в доменную
                var userToUpdate = new UserMinimal
                {
                    Id = model.Id,
                    Username = model.Username,
                    Email = model.Email
                };

                var result = _admin.EditUser(userToUpdate);
                if (!result.Status)
                {
                    TempData["Message"] = result.StatusMsg ?? "Ошибка при обновлении пользователя";
                    TempData["AlertType"] = "danger";
                    return View(model);
                }

                TempData["Message"] = "Пользователь успешно обновлён";
                TempData["AlertType"] = "success";
                return RedirectToAction("Clients");
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Ошибка: {ex.Message}";
                TempData["AlertType"] = "danger";
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUser(int id)
        {
            try
            {
                var result = _admin.DeleteUser(id);
                if (!result.Status)
                {
                    TempData["Message"] = result.StatusMsg ?? "Ошибка при удалении пользователя";
                    TempData["AlertType"] = "danger";
                }
                else
                {
                    TempData["Message"] = "Пользователь успешно удалён";
                    TempData["AlertType"] = "success";
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Ошибка: {ex.Message}";
                TempData["AlertType"] = "danger";
            }

            return RedirectToAction("Clients");
        }
        [HttpGet]
        public ActionResult ChangeRole(int id)
        {
            var result = _admin.GetUserById(id);
            if (!result.Status || result.User == null)
            {
                TempData["Message"] = result.StatusMsg ?? "Пользователь не найден";
                TempData["AlertType"] = "warning";
                return RedirectToAction("Clients");
            }

            var model = new ChangeRoleViewModel
            {
                Id = result.User.Id,
                Username = result.User.Username,
                CurrentRole = result.User.Level.ToString(), // ✅ преобразуем URole в string
                AvailableRoles = Enum.GetNames(typeof(URole)).ToList() // ✅ список всех ролей
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeUserRole(ChangeRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Данные некорректны";
                TempData["AlertType"] = "warning";
                model.AvailableRoles = Enum.GetNames(typeof(URole)).ToList(); // ⬅ добавляем для повторного отображения формы
                return View("ChangeRole", model);
            }

            if (!Enum.TryParse(model.NewRole, out URole newRole))
            {
                TempData["Message"] = "Недопустимая роль";
                TempData["AlertType"] = "danger";
                model.AvailableRoles = Enum.GetNames(typeof(URole)).ToList();
                return View("ChangeRole", model);
            }

            var result = _admin.ChangeUserRole(model.Id, newRole);
            if (!result.Status)
            {
                TempData["Message"] = result.StatusMsg ?? "Не удалось изменить роль";
                TempData["AlertType"] = "danger";
                model.AvailableRoles = Enum.GetNames(typeof(URole)).ToList();
                return View("ChangeRole", model);
            }

            TempData["Message"] = "Роль успешно изменена";
            TempData["AlertType"] = "success";
            return RedirectToAction("Clients");
        }

    }
}
