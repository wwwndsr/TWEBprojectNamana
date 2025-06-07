using webNamana.Domain.Entities.User;

namespace webNamana.BusinessLogic.Interfaces
{
    public interface IUserBL
    {
        UserMinimal GetUserInfoById(int id);
        UserAuthResult EditUserProfile(UserMinimal data); 
    }
}
