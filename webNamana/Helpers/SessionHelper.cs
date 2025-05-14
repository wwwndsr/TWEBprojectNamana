using System;
using System.Web;
using System.Web.SessionState;

namespace webNamana.Helpers
{
    public static class SessionHelper
    {
        private const string USERNAME_KEY = "Username";
        private const string LAST_ACTIVITY_KEY = "LastActivity";
        private const int SESSION_TIMEOUT_MINUTES = 30;

        public static void SetUserSession(string username)
        {
            HttpContext.Current.Session[USERNAME_KEY] = username;
            HttpContext.Current.Session[LAST_ACTIVITY_KEY] = DateTime.UtcNow;
        }

        public static string GetCurrentUsername()
        {
            return HttpContext.Current.Session[USERNAME_KEY]?.ToString();
        }

        public static bool IsUserLoggedIn()
        {
            var username = GetCurrentUsername();
            if (string.IsNullOrEmpty(username))
                return false;

            var lastActivity = HttpContext.Current.Session[LAST_ACTIVITY_KEY] as DateTime?;
            if (!lastActivity.HasValue)
                return false;

            // Check if session has expired
            if ((DateTime.UtcNow - lastActivity.Value).TotalMinutes > SESSION_TIMEOUT_MINUTES)
            {
                ClearSession();
                return false;
            }

            // Update last activity
            HttpContext.Current.Session[LAST_ACTIVITY_KEY] = DateTime.UtcNow;
            return true;
        }

        public static void ClearSession()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }

        public static void RequireAuthentication()
        {
            if (!IsUserLoggedIn())
            {
                HttpContext.Current.Response.Redirect("~/Account/Login");
            }
        }
    }
} 