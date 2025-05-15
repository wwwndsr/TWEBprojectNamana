using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using webNamana.Models;


namespace webNamana.Controllers
{
    public class UserController : Controller
    {
        public ActionResult UserPage()
        {
            var userModel = new UserDashboard
            {
                Username = "JohnDoe",
                Email = "johndoe@example.com",
                CartItems = new List<string> { "Item 1", "Item 2", "Item 3" }
            };

            return View(userModel);
        }
    }
}