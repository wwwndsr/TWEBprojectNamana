using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webNamana.BusinessLogic.Core;
using webNamana.BusinessLogic.Interfaces;
using webNamana.Domain.Entities.User;
using System.Web;

namespace webNamana.BusinessLogic
{
    class SessionBL : UserApi, ISession
    {
        public UserAuthResult UserRegister(UDbTable data)
        {
            return UserRegisterAction(data);
        }

        public UserAuthResult UserLogin(UDbTable data)
        {
            return UserLoginAction(data);
        }

        public HttpCookie GenCookie(string mail)
        {
            return Cookie(mail);
        }

        public UserMinimal GetUserByCookie(string httpCookieValue)
        {
            return UserCookie(httpCookieValue);
        }

        public bool SignOut(string httpCookieValue)
        {
            return SignOutAction(httpCookieValue);
        }

        public bool Login(string username, string password)
        {
            var user = new UDbTable
            {
                Username = username,
                Password = password
            };
            var result = UserLoginAction(user);
            return result.Status;
        }

        public void Logout()
        {
            var cookie = HttpContext.Current.Request.Cookies["WNCNN"];
            if (cookie != null)
            {
                SignOut(cookie.Value);
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public bool IsSessionActive()
        {
            var cookie = HttpContext.Current.Request.Cookies["WNCNN"];
            if (cookie == null) return false;
            
            var user = GetUserByCookie(cookie.Value);
            return user != null;
        }
    }
}
