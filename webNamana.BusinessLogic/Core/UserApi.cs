using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using webNamana.BusinessLogic.DBModel;
using webNamana.Domain.Entities.User;
using webNamana.Domain.Enums;
using webNamana.Helpers;

namespace webNamana.BusinessLogic.Core
{
    public class UserApi
    {
        private const string CookieName = "X-KEY";

        public UserAuthResult UserRegisterAction(UDbTable data)
        {
            var result = new UserAuthResult();

            if (string.IsNullOrEmpty(data.Password) || data.Password.Length < 8)
            {
                return new UserAuthResult { Status = false, StatusMsg = "Password must be at least 8 characters long", StatusKey = "Password" };
            }

            if (string.IsNullOrEmpty(data.Username))
            {
                return new UserAuthResult { Status = false, StatusMsg = "Username cannot be empty", StatusKey = "Username" };
            }

            var validate = new EmailAddressAttribute();
            if (!validate.IsValid(data.Email))
            {
                return new UserAuthResult { Status = false, StatusMsg = "Email is not valid", StatusKey = "Email" };
            }

            using (var db = new UserContext())
            {
                var userExists = db.Users.Any(u => u.Email == data.Email || u.Username == data.Username);
                if (userExists)
                {
                    return new UserAuthResult { Status = false, StatusMsg = "User with such email or username already exists", StatusKey = "Email" };
                }

                data.RegisterTime = DateTime.Now;
                data.LastLogin = DateTime.Now;
                data.Password = LoginHelper.HashGen(data.Password);
                data.Level = URole.User;

                db.Users.Add(data);
                db.SaveChanges();
            }

            return new UserAuthResult { Status = true, StatusMsg = "User registered successfully" };
        }

        public UserAuthResult UserLoginAction(UDbTable data)
        {
            var result = new UserAuthResult();
            var validate = new EmailAddressAttribute();

            if (string.IsNullOrEmpty(data.Password) || data.Password.Length < 8 || !validate.IsValid(data.Email))
            {
                return new UserAuthResult { Status = false, StatusMsg = "Email or Password is not valid", StatusKey = "Email" };
            }

            using (var db = new UserContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Email == data.Email);
                if (user == null)
                {
                    return new UserAuthResult { Status = false, StatusMsg = "User not found", StatusKey = "Email" };
                }

                if (user.Password != LoginHelper.HashGen(data.Password))
                {
                    return new UserAuthResult { Status = false, StatusMsg = "Email or Password is not valid", StatusKey = "Email" };
                }

                user.LastLogin = DateTime.Now;
                user.LastIp = data.LastIp;

                db.Users.AddOrUpdate(user);
                db.SaveChanges();

                return new UserAuthResult { Status = true, StatusMsg = "User logged in successfully" };
            }
        }

        public HttpCookie Cookie(string email)
        {
            var cookie = new HttpCookie(CookieName)
            {
                Value = CookieGenerator.Create(email),
                HttpOnly = true,
                Secure = HttpContext.Current.Request.IsSecureConnection,
                Expires = DateTime.Now.AddDays(1),
                Path = "/"
            };

            var validate = new EmailAddressAttribute();
            if (!validate.IsValid(email))
            {
                throw new Exception("Invalid email");
            }

            using (var db = new SessionContext())
            {
                var existing = db.Sessions.FirstOrDefault(s => s.Email == email);
                if (existing == null)
                {
                    db.Sessions.Add(new Session
                    {
                        Email = email,
                        CookieString = cookie.Value,
                        ExpireTime = DateTime.Now.AddDays(1)
                    });
                }
                else
                {
                    existing.CookieString = cookie.Value;
                    existing.ExpireTime = DateTime.Now.AddDays(1);
                    db.Sessions.AddOrUpdate(existing);
                }

                db.SaveChanges();
            }

            return cookie;
        }

        public bool SignOutAction(string cookie)
        {
            using (var db = new SessionContext())
            {
                var session = db.Sessions.FirstOrDefault(s => s.CookieString == cookie);
                if (session == null) return false;

                db.Sessions.Remove(session);
                db.SaveChanges();
                return true;
            }
        }

        public UserMinimal UserCookie(string cookie)
        {
            using (var db = new SessionContext())
            {
                var session = db.Sessions.FirstOrDefault(s => s.CookieString == cookie);
                if (session == null || session.ExpireTime < DateTime.Now)
                {
                    if (session != null) SignOutAction(cookie);
                    return null;
                }

                using (var userDb = new UserContext())
                {
                    var user = userDb.Users.FirstOrDefault(u => u.Email == session.Email);
                    if (user == null) return null;

                    return new UserMinimal
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Email = user.Email,
                        Level = user.Level
                    };
                }
            }
        }

        public UserAuthResult UpdateProfileAction(UDbTable data)
        {
            var result = new UserAuthResult();

            using (var db = new UserContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Id == data.Id);
                if (user == null)
                {
                    return new UserAuthResult { Status = false, StatusMsg = "User not found" };
                }

                if (!string.IsNullOrEmpty(data.Email))
                {
                    var validate = new EmailAddressAttribute();
                    if (!validate.IsValid(data.Email))
                    {
                        return new UserAuthResult { Status = false, StatusMsg = "Email is not valid", StatusKey = "Email" };
                    }

                    var emailTaken = db.Users.Any(u => u.Email == data.Email && u.Id != data.Id);
                    if (emailTaken)
                    {
                        return new UserAuthResult { Status = false, StatusMsg = "Email is already in use", StatusKey = "Email" };
                    }

                    user.Email = data.Email;
                }

                if (!string.IsNullOrEmpty(data.Username))
                {
                    user.Username = data.Username;
                }

                if (!string.IsNullOrEmpty(data.Password))
                {
                    if (data.Password.Length < 8)
                    {
                        return new UserAuthResult { Status = false, StatusMsg = "Password must be at least 8 characters long", StatusKey = "Password" };
                    }

                    user.Password = LoginHelper.HashGen(data.Password);
                }

                db.SaveChanges();
            }

            return new UserAuthResult { Status = true, StatusMsg = "Profile updated successfully" };
        }

        public UDbTable GetUserById(int id)
        {
            using (var db = new UserContext())
            {
                return db.Users.FirstOrDefault(u => u.Id == id);
            }
        }
    }
}
