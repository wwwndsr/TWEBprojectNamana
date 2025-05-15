using System.Web;
using webNamana.Domain.Entities.User;

namespace webNamana.BusinessLogic.Interfaces
{
    public interface ISession
    {
        UserAuthResult UserRegister(UDbTable data);
        UserAuthResult UserLogin(UDbTable data);
        HttpCookie GenCookie(string mail);
        UserMinimal GetUserByCookie(string cookie);
        bool SignOut(string cookie);
        bool Login(string username, string password);
        void Logout();
        bool IsSessionActive();
    }
}
