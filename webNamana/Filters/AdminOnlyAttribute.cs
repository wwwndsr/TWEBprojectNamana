using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace webNamana.Filters
{
    namespace webNamana.Filters
    {
        public class AdminOnlyAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                var userRole = "Guest";
                if (SessionHelper.User is UserInfo userInfo && userInfo.Role != null)
                {
                    userRole = userInfo.Role;
                }
                else filterContext.Result = new RedirectResult("~/Error/AccessDenied");

                if (userRole != "Admin")
                {
                    filterContext.Result = new RedirectResult("~/Error/AccessDenied");
                }

                base.OnActionExecuting(filterContext);
            }
        }
    }
}