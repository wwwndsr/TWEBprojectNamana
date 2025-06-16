using System;
using System.Collections.Generic;
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


        // GET: /Training
        public ActionResult Index()
        {
            var trainings = _trainingService.GetAllTrainings();
            return View(trainings);
        }

        // GET: /Training/Details/5
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Training ID is required");

            var training = _trainingService.GetTrainingById(id.Value);
            if (training == null)
                return HttpNotFound($"Training with ID {id.Value} not found");

            return View(training);
        }

        // GET: /Training/Create
        public ActionResult Create()
        {
            return View(new TrainingEntity());
        }

        // POST: /Training/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TrainingEntity model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool added = _trainingService.AddTraining(model);
            if (added)
            {
                TempData["Message"] = "Training successfully added.";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Error adding training.");
                return View(model);
            }
        }

        // GET: /Training/Edit/5
        public ActionResult Edit(int id)
        {
            var training = _trainingService.GetTrainingById(id);
            if (training == null)
                return HttpNotFound();

            return View(training);
        }

        // POST: /Training/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TrainingEntity model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool updated = _trainingService.UpdateTraining(id, model);
            if (!updated)
            {
                ModelState.AddModelError("", "Error updating training.");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        // GET: /Training/Delete/5
        public ActionResult Delete(int id)
        {
            var training = _trainingService.GetTrainingById(id);
            if (training == null)
                return HttpNotFound();

            return View(training);
        }

        // POST: /Training/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bool deleted = _trainingService.DeleteTraining(id);
            if (!deleted)
            {
                ModelState.AddModelError("", "Error deleting training.");
                return View(_trainingService.GetTrainingById(id));
            }

            return RedirectToAction("Index");
        }


        // --- Регистрация тренировки ---

        // GET: /Training/Register
        public ActionResult Register(string trainingName = "", string time = "")
        {
            var model = new TrainingRegistration
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

            // Для ViewBag можно использовать список названий тренировок из базы
            var trainings = _trainingService.GetAllTrainings();
            ViewBag.AvailableTrainings = trainings.ConvertAll(t => t.TrainingName);

            ViewBag.SelectedTraining = trainingName;
            ViewBag.SelectedDateTime = time;

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
