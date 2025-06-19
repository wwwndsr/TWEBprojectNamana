using System;
using System.Collections.Generic;
using System.Linq;
using webNamana.BusinessLogic.Core;
using webNamana.BusinessLogic.DBModel;
using webNamana.BusinessLogic.Interfaces;
using webNamana.Domain.Entities.User;
using webNamana.Domain.Enums;
using webNamana.Helpers;

namespace webNamana.BusinessLogic.Services
{
    public class UserService : UserApi, IUserService
    {
        public UDbTable GetUserByUsername(string username)
        {
            //return GetUserByUsernameAction(username);
            // Костыльная реализация для теста
            if (username == "fakeAdmin")
            {
                return new UDbTable
                {
                    Username = "fakeAdmin",
                    Email = "admin@example.com",
                    Level = URole.Admin
                };
            }

            if (username == "fakeUser")
            {
                return new UDbTable
                {
                    Username = "fakeUser",
                    Email = "user@example.com",
                    Level = URole.User
                };
            }

            return null;
        }

        public bool UpdateUserProfile(string username, UDbTable updatedUser)
        {
            return UpdateUserProfileAction(username, updatedUser);
        }

        public bool ChangePassword(string username, string currentPassword, string newPassword)
        {
            return ChangePasswordAction(username, currentPassword, newPassword);
        }

        public bool ValidateUserCredentials(string username, string password)
        {
            return ValidateUserCredentialsAction(username, password);
        }

        public bool CreateUser(UDbTable newUser)
        {
            return CreateUserAction(newUser);
        }
    }
}

