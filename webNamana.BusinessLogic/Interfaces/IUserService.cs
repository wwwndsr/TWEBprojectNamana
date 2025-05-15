using webNamana.Domain.Entities.User;

namespace webNamana.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        UDbTable GetUserByEmail(string email);
        bool UpdateUserProfile(string currentEmail, string newUsername, string newEmail);
        bool ChangePassword(string email, string currentPassword, string newPassword);
        bool VerifyPassword(string storedPassword, string providedPassword);
    }
} 