using System;
using System.Collections.Generic;
using System.Web.Mvc;
using webNamana.Models;

namespace webNamana.Controllers
{
    public class TrainingController : Controller
    {
        // Доступные тренировки и расписание времён
        private static readonly List<string> availableTrainings = new List<string>
        {
            "Rowing Workout", "Yoga Class", "CrossFit", "Pilates", "HIIT Session"
        };

        private static readonly Dictionary<string, List<DateTime>> trainingTimes = new Dictionary<string, List<DateTime>>
        {
            { "Rowing Workout", new List<DateTime> { DateTime.Today.AddHours(10), DateTime.Today.AddHours(12) } },
            { "Yoga Class", new List<DateTime> { DateTime.Today.AddHours(14), DateTime.Today.AddHours(16) } },
            { "CrossFit", new List<DateTime> { DateTime.Today.AddHours(9), DateTime.Today.AddHours(18) } },
            { "Pilates", new List<DateTime> { DateTime.Today.AddHours(11), DateTime.Today.AddHours(15) } },
            { "HIIT Session", new List<DateTime> { DateTime.Today.AddHours(17), DateTime.Today.AddHours(19) } }
        };

        // -------------------- Регистрация --------------------

        // GET: /Training/Register
        public ActionResult Register(string training = "", string time = "")
        {
            var model = new TrainingRegistration
            {
                TrainingType = training,
                CreatedAt = DateTime.Now
            };

            if (!string.IsNullOrEmpty(time) && DateTime.TryParse(time, out var parsedDateTime))
            {
                model.RegistrationDate = parsedDateTime.Date;
                model.TrainingTime = parsedDateTime.TimeOfDay;
            }
            else
            {
                model.RegistrationDate = DateTime.Now.Date;
                model.TrainingTime = DateTime.Now.TimeOfDay;
            }

            ViewBag.SelectedTraining = training;
            ViewBag.SelectedDateTime = time;

            ViewBag.AvailableTrainings = availableTrainings;

            ViewBag.AvailableTimes = !string.IsNullOrEmpty(training) && trainingTimes.ContainsKey(training)
                ? trainingTimes[training]
                : new List<DateTime>();

            return View(model);
        }

        // POST: /Training/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(TrainingRegistration model, string TrainingDateTime)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(TrainingDateTime) && DateTime.TryParse(TrainingDateTime, out var selectedDateTime))
                    {
                        model.RegistrationDate = selectedDateTime.Date;
                        model.TrainingTime = selectedDateTime.TimeOfDay;
                    }

                    model.CreatedAt = DateTime.Now;
                    model.IsConfirmed = false;

                    TempData["SuccessMessage"] = "Your training registration has been received successfully!";
                    return RedirectToAction("Success");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "An error occurred while processing your registration. Please try again.");
                }
            }

            ViewBag.AvailableTrainings = availableTrainings;
            ViewBag.AvailableTimes = trainingTimes.ContainsKey(model.TrainingType)
                ? trainingTimes[model.TrainingType]
                : new List<DateTime>();

            return View(model);
        }

        // GET: /Training/Success
        public ActionResult Success()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAvailableTimes(string trainingType)
        {
            var times = new List<string>();

            if (!string.IsNullOrEmpty(trainingType) && trainingTimes.ContainsKey(trainingType))
            {
                foreach (var time in trainingTimes[trainingType])
                {
                    times.Add(time.ToString("yyyy-MM-ddTHH:mm")); // Формат для input type="datetime-local"
                }
            }

            return Json(times, JsonRequestBehavior.AllowGet);
        }

        // -------------------- Расписание --------------------

        // GET: /Training/Schedule
        public ActionResult Schedule()
        {
            // Пока просто отображаем страницу без БД
            return View();
        }
    }
}
