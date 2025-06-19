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
            return GetUserByUsernameAction(username);

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
        public bool GetUserByEmail(UDbTable newUser)
        {
            return CreateUserAction(newUser);
        }

        public bool ValidateUserCredentialsByEmail(string email, string password)
        {
            return ValidateUserCredentialsByEmail(email, password);
        }

        public void UpdateUserLoginData(string email, string ip)
        {
            UpdateUserLoginData(email, ip);
        }

    }
}

