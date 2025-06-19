using System.Collections.Generic;
using webNamana.Domain.Entities.User;

namespace webNamana.BusinessLogic.Interfaces
{
    public interface IUserBL
    {
        UDbTable GetUserByUsername(string username);
        bool UpdateUserProfile(string username, UDbTable updatedUser);
        bool ChangePassword(string username, string currentPassword, string newPassword);
        bool ValidateUserCredentials(string username, string password);
        bool CreateUser(UDbTable newUser);
        UDbTable GetUserByEmail(string email);
        bool ValidateUserCredentialsByEmail(string email, string password);
        void UpdateUserLoginData(string email, string ip);


    }
}
