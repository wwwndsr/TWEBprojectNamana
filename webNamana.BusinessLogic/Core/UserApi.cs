using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace webNamana.BusinessLogic.Core
{
    public class UserApi
    {
        public bool ValidateUser(string username, string password)
        {
            var result = new UserAuthResult();
            if (data.Password.Length < 8)
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

            UDbTable user;

            using (var db = new UserContext())
            {
                user = db.Users.FirstOrDefault(u => u.Email == data.Email || u.Username == data.Username);
            }

            if (user != null)
            {
                result.Status = false;
                result.StatusMsg = "User already exists";
                result.StatusKey = "Email";
                return result;
            }

            data.RegisterTime = DateTime.Now;
            data.LastLogin = DateTime.Now;
            data.Password = LoginHelper.HashGen(data.Password);

            using (var db = new UserContext())
            {
                db.Users.Add(data);
                db.SaveChanges();
            }

            result.Status = true;
            result.StatusMsg = "User registered successfully";
            return result;
        }

        public UserAuthResult UserLoginAction(UDbTable data)
        {
            UserAuthResult result = new UserAuthResult();

            var validate = new EmailAddressAttribute();

            if (data.Password.Length < 8 || !validate.IsValid(data.Email))
            {
                result.Status = false;
                result.StatusMsg = "Email or Password is not valid";
                result.StatusKey = "Email";
                return result;
            }

            UDbTable user;

            using (var db = new UserContext())
            {
                user = db.Users.FirstOrDefault(u => u.Email == data.Email);
            }

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

            using (var db = new UserContext())
            {
                db.Users.AddOrUpdate(user);
                db.SaveChanges();
            }

            result.Status = true;
            result.StatusMsg = "User logged in successfully";
            return result;
        }

        public HttpCookie Cookie(string email)
        {
            var httpCookie = new HttpCookie("WNCNN")
            {
                Value = CookieGenerator.Create(email)
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
                    }
                    else
                    {
                        current.CookieString = httpCookie.Value;
                        current.ExpireTime = DateTime.Now.AddDays(1);
                    }

                    db.Sessions.AddOrUpdate(current);
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

            UDbTable user;
            using (var db = new UserContext())
            {
                user = db.Users.FirstOrDefault(u => u.Email == session.Email);
            }

            if (user == null) return null;

            var um = new UserMinimal()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Level = user.Level
            };

            return um;
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
