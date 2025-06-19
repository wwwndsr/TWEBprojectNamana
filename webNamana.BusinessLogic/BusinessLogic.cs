using webNamana.BusinessLogic.Interfaces;
using webNamana.BusinessLogic;
using webNamana.BusinessLogic.BLogic;
using webNamana.BusinessLogic.Services; 

namespace webNamana.BusinessLogic
{
    public class BusinessLogic
    {
        public ISession GetSessionBL()
        {
            return new SessionBL();
        }

        public IUserBL GetUserBL()
        {
            return new UserBL();
        }

        public IProductBL GetProductBL()
        {
            return new ProductBL();
        }
        s
        public IAdminBL GetAdminBL()
        {
            return new AdminBL();
        }

        public ITrainingBL GetTrainingBL()
        {
            return new TrainingBL(); 
        }


    }
}
