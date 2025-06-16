using System.Collections.Generic;
using System.Web.Mvc;
using webNamana.Models;
using webNamana.BusinessLogic.Interfaces;
using webNamana.Domain.Entities.User;
using webNamana.Filters;
using System;

namespace webNamana.WebApp.Controllers
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

                return View(result.User);
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
        public ActionResult EditUser(UserMinimal model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Данные некорректны";
                TempData["AlertType"] = "warning";
                return View(model);
            }

            try
            {
                var result = _admin.EditUser(model);
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
    }
}
