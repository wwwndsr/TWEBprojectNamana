using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using webNamana.BusinessLogic.Interfaces;
using webNamana.Domain.Entities.User;
using webNamana.Domain.Enums;
using webNamana.Filters;
using webNamana.Helpers;
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

        [AdminOnly]
        public ActionResult AdminPage()
        {
            try
            {
                var encryptedCookie = Request.Cookies["X-KEY"]?.Value;
                if (string.IsNullOrWhiteSpace(encryptedCookie))
                    return RedirectToAction("Login", "Account");

                string email;
                try
                {
                    email = CookieGenerator.Validate(encryptedCookie);
                }
                catch
                {
                    return RedirectToAction("Login", "Account");
                }

                // Получаем пользователя через IUserService (как в фильтре)
                var user = _admin.GetUserByEmail(email); // Добавь этот метод в IAdminBL, если его нет
                if (user == null)
                {
                    TempData["Message"] = "Пользователь не найден";
                    TempData["AlertType"] = "danger";
                    return RedirectToAction("Login", "Account");
                }

                var allUsersResult = _admin.GetAllUsers();
                int totalUsers = (allUsersResult.Status && allUsersResult.Users != null)
                    ? allUsersResult.Users.Count
                    : 0;

                var model = new AdminDashboard
                {
                    Username = user.Username,
                    RecentActivity = new List<string>
            {
                "Пользователь вошёл в систему",
                "Отредактировал профиль",
                "Изменил роль пользователя"
            },
                    TotalUsers = totalUsers,
                    TotalProducts = 234,
                    TotalOrders = 1234
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Ошибка загрузки панели администратора: {ex.Message}";
                TempData["AlertType"] = "danger";
                return RedirectToAction("Clients");
            }
        }



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
                CurrentRole = result.User.Level.ToString(),
                AvailableRoles = Enum.GetNames(typeof(URole)).ToList()
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
                model.AvailableRoles = Enum.GetNames(typeof(URole)).ToList();
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
