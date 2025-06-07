using System;
using System.Web.Mvc;
using webNamana.Domain.Entities.User;


namespace webNamana.Web.Controllers
    {
        public class AccountController : Controller
        {
            private readonly BusinessLogic.Core.UserApi _userApi = new BusinessLogic.Core.UserApi();

            // GET: /Account/Login
            public ActionResult Login()
            {
                return View();
            }

            // POST: /Account/Login
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Login(Domain.Entities.User.UDbTable model)
            {
                if (!ModelState.IsValid)
                    return View(model);

                model.LastIp = Request.UserHostAddress;

                var result = _userApi.UserLoginAction(model);
                if (!result.Status)
                {
                    ModelState.AddModelError(result.StatusKey, result.StatusMsg);
                    return View(model);
                }

                var cookie = _userApi.Cookie(model.Email);
                Response.Cookies.Add(cookie);

                return RedirectToAction("Index", "Home");
            }

            // GET: /Account/Register
            public ActionResult Register()
            {
                return View();
            }

            // POST: /Account/Register
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Register(UDbTable model)
            {
                if (!ModelState.IsValid)
                    return View(model);

                var result = _userApi.UserRegisterAction(model);
                if (!result.Status)
                {
                    ModelState.AddModelError(result.StatusKey, result.StatusMsg);
                    return View(model);
                }

                var cookie = _userApi.Cookie(model.Email);
                Response.Cookies.Add(cookie);

                return RedirectToAction("Index", "Home");
            }

            // GET: /Account/Profile
            [Authorize]
        public ActionResult UserProfile()
        {
            var user = GetCurrentUser();
            if (user == null)
                return RedirectToAction("Login");

            return View(user);
        }

        // POST: /Account/Profile
        [HttpPost]
            [ValidateAntiForgeryToken]
            [Authorize]
            public ActionResult EditProfile(UDbTable model)
            {
                var currentUser = GetCurrentUser();
                if (currentUser == null)
                    return RedirectToAction("Login");

                model.Id = currentUser.Id;

                var result = _userApi.UpdateProfileAction(model);
                if (!result.Status)
                {
                    ModelState.AddModelError(result.StatusKey, result.StatusMsg);
                    return View(model);
                }

                ViewBag.SuccessMessage = "Profile updated successfully.";
                return View(model);
            }

            // GET: /Account/Logout
            public ActionResult Logout()
            {
                var cookie = Request.Cookies["WNCNN"];
                if (cookie != null)
                {
                    _userApi.SignOutAction(cookie.Value);
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }

                return RedirectToAction("Login");
            }

            private UserMinimal GetCurrentUser()
            {
                var cookie = Request.Cookies["WNCNN"];
                if (cookie == null)
                    return null;

                return _userApi.UserCookie(cookie.Value);
            }
        }
    }

