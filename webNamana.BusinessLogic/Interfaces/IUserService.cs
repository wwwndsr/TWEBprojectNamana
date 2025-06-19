using System.Collections.Generic;
using webNamana.Domain.Entities.User;

namespace webNamana.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        UDbTable GetUserByUsername(string username);
        UDbTable GetUserByEmail(string email);                       // добавлено
        bool UpdateUserProfile(string username, UDbTable updatedUser);
        bool ChangePassword(string username, string currentPassword, string newPassword);
        bool ValidateUserCredentials(string username, string password);
        bool ValidateUserCredentialsByEmail(string email, string password);  // добавлено
        bool CreateUser(UDbTable newUser);
        bool UpdateUser(UDbTable user);                             // добавлено
        void UpdateUserLoginData(string email, string ip);
    }

}
