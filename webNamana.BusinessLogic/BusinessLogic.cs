using System.Collections.Generic;
using webNamana.BusinessLogic.BLogic;
using webNamana.BusinessLogic.Interfaces;

namespace webNamana.BusinessLogic
{
    public class BusinessLogic
    {
        public ISession GetSessionBL()
        {
            return new SessionBL();
        }
   
    public IUserBL GetUserBl()
        {
            return new UserBL();
        }
        public IAdminBL GetAdminBl()
        {
            return new AdminBL();
        }
    }
}
