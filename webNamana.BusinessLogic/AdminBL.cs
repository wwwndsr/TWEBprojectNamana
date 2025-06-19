using System;
using webNamana.BusinessLogic.Core;
using webNamana.BusinessLogic.DBModel;
using webNamana.BusinessLogic.Interfaces;
using webNamana.Domain.Entities.User;
using webNamana.Domain.Enums;

namespace webNamana.BusinessLogic
{
    internal class AdminBL : AdminApi, IAdminBL
    {
        public AdminAuthResult GetAllUsers()
        {
            // Просто делегируем вызов базовому методу
            return base.GetAllUsersAction();
        }

        public AdminAuthResult GetUserById(int id)
        {
            return base.GetUserByIdAction(id);
        }
        /*public AdminAuthResult GetUserByUsername(string username)
        {
            return base.GetUserByUsernameAction(username);
        }
        */
        public AdminAuthResult GetUserByUsername(string username)
        {
            if (username == "fakeAdmin")
            {
                return new AdminAuthResult
                {
                    Status = true,
                    User = new UserMinimal
                    {
                        Id = 1,
                        Username = "fakeAdmin",
                        Email = "admin@example.com",
                        Level = URole.Admin,
                        LastLogin = DateTime.Now,
                        RegisterTime = DateTime.Now.AddMonths(-1)
                    }
                };
            }

            return new AdminAuthResult
            {
                Status = false,
                StatusMsg = "Пользователь не найден"
            };
        }

        public UDbTable GetUserByEmail(string email)
        {
            return GetUserByEmailAction(email);
        }

        public AdminAuthResult EditUser(UserMinimal user)
        {
            // Подготовим объект UDbTable для обновления
            var data = new UDbTable
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Level = user.Level
            };

            return base.UpdateUserAction(data);
        }

        public AdminAuthResult DeleteUser(int id)
        {
            return base.DeleteUserAction(id);
        }

        public AdminAuthResult ChangeUserRole(int id, URole newRole)
        {
            return base.ChangeUserRoleAction(id, newRole);
        }
    }
}
