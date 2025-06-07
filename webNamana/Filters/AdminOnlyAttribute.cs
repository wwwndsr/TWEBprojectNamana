using System.Web.Mvc;
using webNamana.Domain.Entities.User;
using webNamana.Helpers;

namespace webNamana.Filters
{
    public class AdminOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userRole = "Guest";
            if (SessionHelper.User is UserMinimal user)
            {
                userRole = user.Level.ToString();
            }
            else
            {
                filterContext.Result = new RedirectResult("~/Error/AccessDenied");
                return;
            }

            if (userRole != "Admin")
            {
                filterContext.Result = new RedirectResult("~/Error/AccessDenied");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
