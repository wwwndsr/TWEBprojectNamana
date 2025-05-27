using System.Linq;
using System.Web;
using System.Web.Mvc;
using webNamana.BusinessLogic.DBModel;
using webNamana.Domain.Enums;

namespace webNamana.Filters
{
    public class AdminOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cookie = HttpContext.Current.Request.Cookies["X-KEY"];
            if (cookie == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }

            var email = cookie.Value;
            using (var db = new UserContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Email == email);
                if (user == null || user.Level != URole.Admin)
                {
                    filterContext.Result = new RedirectResult("~/Error/AccessDenied");
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
