using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webNamana.Domain.Entities.User;
using webNamana.Helpers;

namespace webNamana.Filters
{
    public class UserOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userRole = "Guest";
            if (SessionHelper.User is UserMinimal userInfo)
            {
                userRole = userInfo.Level.ToString();
            }
            else
            {
                filterContext.Result = new RedirectResult("~/Error/AccessDenied");
                return;
            }

            if (userRole == "Guest")
            {
                filterContext.Result = new RedirectResult("~/Error/AccessDenied");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
