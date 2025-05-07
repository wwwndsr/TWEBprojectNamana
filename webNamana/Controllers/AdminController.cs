using System.Collections.Generic;
using System.Web.Mvc;
using webNamana.Models;
namespace webNamana.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult AdminPage()
        {
            var adminModel = new AdminDashboard
            {
                Username = "Admin",
                TotalUsers = 1200,
                TotalProducts = 150,
                TotalOrders = 800,
                RecentActivity = new List<string>
                {
                    "User 'John Doe' registered",
                    "Product 'Super Widget' added",
                    "Order #1523 completed"
                }
            };

            return View(adminModel);
        }
    }
}