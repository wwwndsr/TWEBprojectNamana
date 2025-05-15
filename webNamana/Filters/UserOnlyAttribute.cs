using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShop.Domain.User.Admin;

namespace webNamana.Filters
{
    public class UserOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userRole = "Guest";
            if (SessionHelper.User is UserInfo userInfo && userInfo.Role != null)
            {
                userRole = userInfo.Role;
            }
            else filterContext.Result = new RedirectResult("~/Error/AccessDenied");

            if (userRole == "Guest")
            {
                filterContext.Result = new RedirectResult("~/Error/AccessDenied");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}