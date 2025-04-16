using System;
using System.Web.Mvc;
using webNamana.Models;

namespace webNamana.Controllers
{
    public class TrainingRegistrationController : Controller
    {
        // GET: TrainingRegistration/Register
        public ActionResult Register(string trainingType = "")
        {
            var model = new TrainingRegistration
            {
                TrainingType = trainingType,
                RegistrationDate = DateTime.Now,
                TrainingTime = DateTime.Now.TimeOfDay,
                CreatedAt = DateTime.Now
            };
            return View(model);
        }

        // POST: TrainingRegistration/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(TrainingRegistration model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Here you would typically save to database
                    model.CreatedAt = DateTime.Now;
                    model.IsConfirmed = false;

                    // Set success message and redirect to success page
                    TempData["SuccessMessage"] = "Your training registration has been received successfully!";
                    return RedirectToAction("Success");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while processing your registration. Please try again.");
                }
            }
            return View(model);
        }

        // GET: TrainingRegistration/Success
        public ActionResult Success()
        {
            return View();
        }
    }
}
