using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using webNamana.BusinessLogic.Interfaces;
using webNamana.BusinessLogic.Services;
using webNamana.Domain.Entities.Product;
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
        private readonly ProductBL _product;

        public AdminController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _admin = bl.GetAdminBL();
            _product = new ProductBL(); // если есть IProductBL — замени
        }

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

                var user = _admin.GetUserByEmail(email);
                if (user == null)
                {
                    TempData["Message"] = "Пользователь не найден";
                    TempData["AlertType"] = "danger";
                    return RedirectToAction("Login", "Account");
                }

                var allUsersResult = _admin.GetAllUsers();
                int totalUsers = allUsersResult.Status && allUsersResult.Users != null
                    ? allUsersResult.Users.Count
                    : 0;

                var totalProducts = _product.GetAllProducts()?.Count ?? 0;

                var model = new AdminDashboard
                {
                    Username = user.Username,
                    RecentActivity = new List<string> { "Раздел в разработке" },
                    TotalUsers = totalUsers,
                    TotalProducts = totalProducts
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Ошибка: {ex.Message}";
                TempData["AlertType"] = "danger";
                return RedirectToAction("ManageUsers");
            }
        }

        public ActionResult ManageUsers()
        {
            var result = _admin.GetAllUsers();
            if (!result.Status)
            {
                TempData["Message"] = result.StatusMsg ?? "Ошибка при получении пользователей";
                TempData["AlertType"] = "danger";
                return View(new List<UserMinimal>());
            }

            return View("ManageUsers", result.Users);
        }

        [HttpGet]
        public ActionResult EditUser(int id)
        {
            var result = _admin.GetUserById(id);
            if (!result.Status || result.User == null)
            {
                TempData["Message"] = "Пользователь не найден";
                TempData["AlertType"] = "warning";
                return RedirectToAction("ManageUsers");
            }

            var user = result.User;
            var model = new EditProfileViewModel
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };

            return View(model);
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

            var updatedUser = new UserMinimal
            {
                Id = model.Id,
                Username = model.Username,
                Email = model.Email
            };

            var result = _admin.EditUser(updatedUser);
            TempData["Message"] = result.Status ? "Пользователь обновлён" : "Ошибка при обновлении";
            TempData["AlertType"] = result.Status ? "success" : "danger";
            return RedirectToAction("ManageUsers");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUser(int id)
        {
            try
            {
                var result = _admin.DeleteUser(id);
                TempData["Message"] = result.Status ? "Пользователь удалён" : "Ошибка при удалении";
                TempData["AlertType"] = result.Status ? "success" : "danger";
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Ошибка: {ex.Message}";
                TempData["AlertType"] = "danger";
            }

            return RedirectToAction("ManageUsers");
        }

        [HttpGet]
        public ActionResult ChangeRole(int id)
        {
            var result = _admin.GetUserById(id);
            if (!result.Status || result.User == null)
            {
                TempData["Message"] = "Пользователь не найден";
                TempData["AlertType"] = "warning";
                return RedirectToAction("ManageUsers");
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
                TempData["Message"] = "Некорректные данные";
                TempData["AlertType"] = "warning";
                model.AvailableRoles = Enum.GetNames(typeof(URole)).ToList();
                return View("ChangeRole", model);
            }

            if (!Enum.TryParse(model.NewRole, out URole newRole))
            {
                TempData["Message"] = "Неверная роль";
                TempData["AlertType"] = "danger";
                model.AvailableRoles = Enum.GetNames(typeof(URole)).ToList();
                return View("ChangeRole", model);
            }

            var result = _admin.ChangeUserRole(model.Id, newRole);
            TempData["Message"] = result.Status ? "Роль изменена" : "Ошибка при изменении роли";
            TempData["AlertType"] = result.Status ? "success" : "danger";
            return RedirectToAction("ManageUsers");
        }

      
    }
}
