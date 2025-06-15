using System.Web.Mvc;
using webNamana.BusinessLogic;
using webNamana.BusinessLogic.Interfaces;
using webNamana.Domain.Enums;

namespace webNamana.Filters
{
    public class AdminOnlyAttribute : ActionFilterAttribute
    {
        private readonly IUserService _userService;

        public AdminOnlyAttribute()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _userService = bl.GetUserService();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cookie = filterContext.HttpContext.Request.Cookies["X-KEY"];
            if (cookie == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }

            var email = cookie.Value;
            var user = _userService.GetUserByUsername(email);

            // если пользователь не найден или роль не админ — доступ запрещён
            if (user == null || user.Level != URole.Admin)
            {
                filterContext.Result = new RedirectResult("~/Error/AccessDenied");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
