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
    public class UserBL : UserApi, IUserBL
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
        public UDbTable GetUserByEmail(string email)
        {
            return GetUserByEmailAction(email);
        }


        public bool ValidateUserCredentialsByEmail(string email, string password)
            {
                return ValidateUserCredentialsByEmailAction(email, password);
            }

        public void UpdateUserLoginData(string email, string ip)
            {
                UpdateUserLoginDataAction(email, ip);
            }


        }


    }


