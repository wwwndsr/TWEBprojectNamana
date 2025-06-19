using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using webNamana.BusinessLogic.Interfaces;
using webNamana.BusinessLogic.Services;
using webNamana.Domain.Entities.Training;
using webNamana.Models;

namespace webNamana.Web.Controllers
{
    public class TrainingController : Controller
    {
        private readonly ITrainingService _trainingService;

        public TrainingController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _trainingService = bl.GetTrainingService();
        }


        // GET: /Training/AdminTrainingList
        public ActionResult AdminTrainingList()
        {
            var trainings = _trainingService.GetAllTrainings();
            var model = new List<TrainingListViewModel>();

            foreach (var t in trainings)
            {
                model.Add(new TrainingListViewModel
                {
                    Id = t.Id,
                    TrainingName = t.TrainingName,
                    DayOfWeek = t.DayOfWeek,
                    StartTime = t.StartTime
                });
            }

            return View(model);
        }

        // GET: /Training/Schedule
        public ActionResult Schedule()
        {
            var trainings = _trainingService.GetAllTrainings();
            var dtoList = trainings.Select(t => new TrainingListViewModel
            {
                Id = t.Id,
                TrainingName = t.TrainingName,
                DayOfWeek = t.DayOfWeek,
                StartTime = t.StartTime
            }).ToList();

            return View(dtoList);
        }

        // GET: /Training/Details/5
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Training ID is required");

            var training = _trainingService.GetTrainingById(id.Value);
            if (training == null)
                return HttpNotFound($"Training with ID {id.Value} not found");

            var model = new TrainingEditViewModel
            {
                Id = training.Id,
                TrainingName = training.TrainingName,
                DayOfWeek = training.DayOfWeek,
                StartTime = training.StartTime
            };

            return View(model);
        }

        // GET: /Training/Create
        public ActionResult Create()
        {
            return View(new TrainingCreateViewModel());
        }

        // POST: /Training/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TrainingCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var entity = new TrainingEntity
            {
                TrainingName = model.TrainingName,
                DayOfWeek = model.DayOfWeek,
                StartTime = model.StartTime
            };

            bool added = _trainingService.AddTraining(entity);
            if (added)
            {
                TempData["Message"] = "Training successfully added!";
                return RedirectToAction("AdminTrainingList");
            }

            ModelState.AddModelError("", "Error adding training.");
            return View(model);
        }

        // GET: Edit
        public ActionResult Edit(int id)
        {
            var training = _trainingService.GetTrainingById(id);
            if (training == null)
                return HttpNotFound();

            var model = new TrainingEditViewModel
            {
                Id = training.Id,
                TrainingName = training.TrainingName,
                DayOfWeek = training.DayOfWeek,
                StartTime = training.StartTime
            };
            return View(model);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TrainingEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_trainingService == null)
                    return new HttpStatusCodeResult(500, "Training service is not available.");

                var updatedEntity = new TrainingEntity
                {
                    Id = model.Id,
                    TrainingName = model.TrainingName,
                    DayOfWeek = model.DayOfWeek,
                    StartTime = model.StartTime
                };

                bool updated = _trainingService.UpdateTraining(model.Id, updatedEntity);
                if (updated)
                {
                    TempData["Message"] = "Training successfully updated";
                    return RedirectToAction("AdminTrainingList");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to update training");
                }
            }
            return View(model);
        }


        // POST: /Training/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            _trainingService.DeleteTraining(id);
            TempData["Message"] = "Training successfully deleted";
            return RedirectToAction("AdminTrainingList");
        }


        // --- Регистрация тренировки ---

        // GET: /Training/Register
        public ActionResult Register(string trainingName = "", string time = "")
        {
            var model = new TrainingRegisterViewModel
            {
                TrainingType = trainingName,
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

            var trainings = _trainingService.GetAllTrainings();
            ViewBag.AvailableTrainings = trainings.ConvertAll(t => t.TrainingName);
            ViewBag.SelectedTraining = trainingName;
            ViewBag.SelectedDateTime = time;

            return View(model);
        }

        // POST: /Training/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(TrainingRegisterViewModel model, string TrainingDateTime)
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
                    ModelState.AddModelError("", "An error occurred while processing your registration.");
                }
            }

            var trainings = _trainingService.GetAllTrainings();
            ViewBag.AvailableTrainings = trainings.ConvertAll(t => t.TrainingName);

            return View(model);
        }

        // GET: /Training/Success
        public ActionResult Success()
        {
            return View();
        }
    }
}