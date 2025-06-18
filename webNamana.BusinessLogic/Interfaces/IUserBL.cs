using System.Web;
using webNamana.Domain.Entities.User;

namespace webNamana.BusinessLogic.Interfaces
{
    public interface IUserBL
    {
        UserMinimal GetUserInfoById(int id);
        UserAuthResult EditUserProfile(UserMinimal data);
        UDbTable GetUserById(int id);
        UserAuthResult UserRegisterAction(UDbTable data);
        UserAuthResult UserLoginAction(UDbTable data);
        HttpCookie Cookie(string email);
        bool SignOutAction(string cookie);
        UserMinimal UserCookie(string cookie);
        UserAuthResult UpdateProfileAction(UDbTable data);
        UDbTable GetUserByUsernameAction(string username);
        bool CreateUserAction(UDbTable newUser);
        bool UpdateUserProfileAction(string username, UDbTable updatedUser);
        bool ChangePasswordAction(string username, string currentPassword, string newPassword);
        bool ValidateUserCredentialsAction(string username, string password);
    }
}
