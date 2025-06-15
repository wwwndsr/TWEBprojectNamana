using System.Web.Mvc;
using webNamana.BusinessLogic;
using webNamana.BusinessLogic.Interfaces;
using webNamana.Domain.Enums;

namespace webNamana.Filters
{
    public class UserOnlyAttribute : ActionFilterAttribute
    {
        private readonly IUserService _userService;

        public UserOnlyAttribute()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _userService = bl.GetUserService();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cookie = filterContext.HttpContext.Request.Cookies["X-KEY"];
            if (cookie == null || string.IsNullOrWhiteSpace(cookie.Value))
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }

            var email = cookie.Value;
            var user = _userService.GetUserByUsername(email);

            // проверяем, что пользователь существует и что его роль user или аdmin (чтобы дать доступ)
            if (user == null || (user.Level != URole.User && user.Level != URole.Admin))
            {
                filterContext.Result = new RedirectResult("~/Error/AccessDenied");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
