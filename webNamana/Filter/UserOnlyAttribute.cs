using System.Web.Mvc;
using webNamana.BusinessLogic;
using webNamana.BusinessLogic.Interfaces;
using webNamana.Domain.Enums;
using webNamana.Helpers;

namespace webNamana.Filters
{
    public class UserOnlyAttribute : ActionFilterAttribute
    {
        private readonly IUserBL _userService;

        public UserOnlyAttribute()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _userService = bl.GetUserBL();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cookie = filterContext.HttpContext.Request.Cookies["X-KEY"];
            if (cookie == null || string.IsNullOrWhiteSpace(cookie.Value))
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }

            string decryptedEmail;
            try
            {
                decryptedEmail = CookieGenerator.Validate(cookie.Value);
            }
            catch
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }

            var user = _userService.GetUserByEmail(decryptedEmail);
            if (user == null || (user.Level != URole.User && user.Level != URole.Admin))
            {
                filterContext.Result = new RedirectResult("~/Error/AccessDenied");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
