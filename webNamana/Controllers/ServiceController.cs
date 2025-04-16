using System.Web.Mvc;

namespace webNamana.Controllers
{
    public class ServiceController : Controller
    {
        // GET: Service
        public ActionResult Service()
        {
            return View();
        }

        public ActionResult CrossfitTraining()
        {
            return View("~/Views/TrainingRegistration/Register.cshtml");
        }

        public ActionResult Fitness()
        {
            return View("~/Views/TrainingRegistration/Register..cshtml");
        }

        public ActionResult DynamicStrengthTraining()
        {
            return View("~/Views/TrainingRegistration/Register..cshtml");
        }

        public ActionResult Health()
        {
            return View("~/Views/TrainingRegistration/Register..cshtml");
        }

        public ActionResult Workout()
        {
            return View("~/Views/TrainingRegistration/Register..cshtml");
        }

        public ActionResult Strategies()
        {
            return View("~/Views/TrainingRegistration/Register..cshtml");
        }
    }
}
