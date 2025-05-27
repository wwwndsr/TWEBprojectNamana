using System;
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

            if (data.Password == null || data.Password.Length < 8)
            {
                result.Status = false;
                result.StatusMsg = "Password must be at least 8 characters long";
                result.StatusKey = "Password";
                return result;
            }

            if (string.IsNullOrEmpty(data.Username))
            {
                result.Status = false;
                result.StatusMsg = "Username cannot be empty";
                result.StatusKey = "Username";
                return result;
            }

            var validate = new EmailAddressAttribute();
            if (!validate.IsValid(data.Email))
            {
                result.Status = false;
                result.StatusMsg = "Email is not valid";
                result.StatusKey = "Email";
                return result;
            }

            using (var db = new UserContext())
            {
                var userExists = db.Users.Any(u => u.Email == data.Email || u.Username == data.Username);
                if (userExists)
                {
                    result.Status = false;
                    result.StatusMsg = "User with such email or username already exists";
                    result.StatusKey = "Email";
                    return result;
                }

                data.RegisterTime = DateTime.Now;
                data.LastLogin = DateTime.Now;
                data.Password = LoginHelper.HashGen(data.Password);

                // Присваиваем роль User по умолчанию
                data.Level = URole.User;

                db.Users.Add(data);
                db.SaveChanges();
            }

            result.Status = true;
            result.StatusMsg = "User registered successfully";
            return result;
        }

        public UDbTable GetUserById(int id)
        {
            using (var db = new UserContext())
            {
                return db.Users.FirstOrDefault(u => u.Id == id);
            }
        }

        public UserAuthResult UserLoginAction(UDbTable data)
        {
            var result = new UserAuthResult();

            var validate = new EmailAddressAttribute();

            if (data.Password == null || data.Password.Length < 8 || !validate.IsValid(data.Email))
            {
                result.Status = false;
                result.StatusMsg = "Email or Password is not valid";
                result.StatusKey = "Email";
                return result;
            }

            using (var db = new UserContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Email == data.Email);

                if (user == null)
                {
                    result.Status = false;
                    result.StatusMsg = "User not found";
                    result.StatusKey = "Email";
                    return result;
                }

                if (user.Password != LoginHelper.HashGen(data.Password))
                {
                    result.Status = false;
                    result.StatusMsg = "Email or Password is not valid";
                    result.StatusKey = "Email";
                    return result;
                }

                user.LastLogin = DateTime.Now;
                user.LasIp = data.LasIp;

                db.Users.AddOrUpdate(user);
                db.SaveChanges();

                result.Status = true;
                result.StatusMsg = "User logged in successfully";
                return result;
            }
        }

        public HttpCookie Cookie(string email)
        {
            var httpCookie = new HttpCookie(CookieName)
            {
                Value = CookieGenerator.Create(email),
                HttpOnly = true,
                Secure = HttpContext.Current.Request.IsSecureConnection,
                Expires = DateTime.Now.AddDays(1),
                Path = "/"
            };

            using (var db = new SessionContext())
            {
                var validate = new EmailAddressAttribute();
                if (validate.IsValid(email))
                {
                    var current = db.Sessions.FirstOrDefault(s => s.Email == email);

                    if (current == null)
                    {
                        current = new Session
                        {
                            Email = email,
                            CookieString = httpCookie.Value,
                            ExpireTime = DateTime.Now.AddDays(1)
                        };
                        db.Sessions.Add(current);
                    }
                    else
                    {
                        current.CookieString = httpCookie.Value;
                        current.ExpireTime = DateTime.Now.AddDays(1);
                        db.Sessions.AddOrUpdate(current);
                    }

                    db.SaveChanges();
                }
                else
                {
                    throw new Exception("Invalid email");
                }
            }
            return httpCookie;
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
            Session session;

            using (var db = new SessionContext())
            {
                session = db.Sessions.FirstOrDefault(s => s.CookieString == cookie);
            }

            if (session == null) return null;

            if (session.ExpireTime < DateTime.Now)
            {
                SignOutAction(cookie);
                return null;
            }

            using (var db = new UserContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Email == session.Email);
                if (user == null) return null;

                return new UserMinimal()
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Level = user.Level
                };
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
                    result.Status = false;
                    result.StatusMsg = "User not found";
                    return result;
                }

                if (!string.IsNullOrEmpty(data.Email))
                {
                    var validate = new EmailAddressAttribute();
                    if (!validate.IsValid(data.Email))
                    {
                        result.Status = false;
                        result.StatusMsg = "Email is not valid";
                        result.StatusKey = "Email";
                        return result;
                    }

                    var emailTaken = db.Users.Any(u => u.Email == data.Email && u.Id != data.Id);
                    if (emailTaken)
                    {
                        result.Status = false;
                        result.StatusMsg = "Email is already taken by another user";
                        result.StatusKey = "Email";
                        return result;
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
                        result.Status = false;
                        result.StatusMsg = "Password must be at least 8 characters long";
                        result.StatusKey = "Password";
                        return result;
                    }
                    user.Password = LoginHelper.HashGen(data.Password);
                }

                db.SaveChanges();

                result.Status = true;
                result.StatusMsg = "Profile updated successfully";
            }

            return result;
        }
    }
}
