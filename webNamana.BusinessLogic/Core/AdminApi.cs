using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using webNamana.BusinessLogic.DBModel;
using webNamana.BusinessLogic.Interfaces;
using webNamana.Domain.Entities.User;
using webNamana.Domain.Enums;

namespace webNamana.BusinessLogic.Core
{
    public class AdminApi
    {
        internal AdminAuthResult GetAllUsersAction()
        {
            var result = new AdminAuthResult();

            using (var db = new UserContext())
            {
                var users = db.Users.Select(u => new UserMinimal
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Level = u.Level,
                    LastLogin = u.LastLogin,
                    RegisterTime = u.RegisterTime
                }).ToList();

                result.Status = true;
                result.StatusMsg = "Users retrieved successfully";
                result.Users = users;
            }

            return result;
        }

        internal AdminAuthResult GetUserByIdAction(int userId)
        {
            var result = new AdminAuthResult();

            using (var db = new UserContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    result.Status = false;
                    result.StatusMsg = "User not found";
                    return result;
                }

                result.Status = true;
                result.StatusMsg = "User retrieved successfully";
                result.User = new UserMinimal
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Level = user.Level,
                    LastLogin = user.LastLogin,
                    RegisterTime = user.RegisterTime
                };
            }

            return result;
        }

        internal AdminAuthResult UpdateUserAction(UDbTable data)
        {
            var result = new AdminAuthResult();

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
                    user.Email = data.Email;

                if (!string.IsNullOrEmpty(data.Username))
                    user.Username = data.Username;

                if (data.Level != 0)
                    user.Level = data.Level;

                db.SaveChanges();
                result.Status = true;
                result.StatusMsg = "User updated successfully";
            }

            return result;
        }

        internal AdminAuthResult DeleteUserAction(int userId)
        {
            var result = new AdminAuthResult();

            using (var db = new UserContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    result.Status = false;
                    result.StatusMsg = "User not found";
                    return result;
                }

                db.Users.Remove(user);
                db.SaveChanges();
                result.Status = true;
                result.StatusMsg = "User deleted successfully";
            }

            return result;
        }
        internal AdminAuthResult ChangeUserRoleAction(int userId, URole newRole)
        {
            var result = new AdminAuthResult();

            using (var db = new UserContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    result.Status = false;
                    result.StatusMsg = "User not found";
                    return result;
                }

                user.Level = newRole;
                db.SaveChanges();
                result.Status = true;
                result.StatusMsg = "User role updated successfully";
            }

            return result;
        }
    }

}