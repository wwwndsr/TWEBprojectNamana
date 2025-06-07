using System;
using webNamana.BusinessLogic.Core;
using webNamana.BusinessLogic.Interfaces;
using webNamana.Domain.Entities.User;

namespace webNamana.BusinessLogic.BLogic
{
    public class UserBL : UserApi, IUserBL
    {
        public UserMinimal GetUserInfoById(int id)
        {
            var userFromDB = GetUserById(id);
            return new UserMinimal
            {
                Id = userFromDB.Id,
                Username = userFromDB.Username,
                Email = userFromDB.Email
            };
        }

        public UserAuthResult EditUserProfile(UserMinimal data)
        {
            var userToUpdate = new UDbTable
            {
                Id = data.Id,
                Username = data.Username,
                Email = data.Email
            };

            var result = UpdateProfileAction(userToUpdate);

            return new UserAuthResult
            {
                Status = result.Status,
                StatusMsg = result.StatusMsg,
                StatusKey = result.StatusKey // если есть
            };
        }

    }
}
