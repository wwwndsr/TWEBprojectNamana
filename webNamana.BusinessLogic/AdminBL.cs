using webNamana.BusinessLogic.Core;
using webNamana.BusinessLogic.Interfaces;
using webNamana.Domain.Entities.User;
using webNamana.Domain.Enums;
using webNamana.BusinessLogic.DBModel;

namespace webNamana.BusinessLogic
{
    internal class AdminBL : AdminApi, IAdminBL
    {
        public AdminAuthResult GetAllUsers()
        {
            // Просто делегируем вызов базовому методу
            return base.GetAllUsersAction();
        }

        public AdminAuthResult GetUserById(int id)
        {
            return base.GetUserByIdAction(id);
        }

        public AdminAuthResult EditUser(UserMinimal user)
        {
            // Подготовим объект UDbTable для обновления
            var data = new UDbTable
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Level = user.Level
            };

            return base.UpdateUserAction(data);
        }

        public AdminAuthResult DeleteUser(int id)
        {
            return base.DeleteUserAction(id);
        }

        public AdminAuthResult ChangeUserRole(int id, URole newRole)
        {
            return base.ChangeUserRoleAction(id, newRole);
        }
    }
}
