using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webNamana.Domain.Entities.User;
using webNamana.Domain.Enums;

namespace webNamana.BusinessLogic.Interfaces
{
    public interface IAdminBL
    {
        AdminAuthResult GetAllUsers();
        AdminAuthResult GetUserById(int id);
        AdminAuthResult EditUser(UserMinimal user);
        AdminAuthResult DeleteUser(int id);
        AdminAuthResult ChangeUserRole(int id, URole newRole);
    }
}
