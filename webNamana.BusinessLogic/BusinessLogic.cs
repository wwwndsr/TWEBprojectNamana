using webNamana.BusinessLogic.Interfaces;
using webNamana.BusinessLogic;
using webNamana.BusinessLogic.Services; 

namespace webNamana.BusinessLogic
{
    public class BusinessLogic
    {
        public ISession GetSessionBL()
        {
            return new SessionBL();
        }

        public IUserService GetUserService()
        {
            return new UserService();
        }

        public IProductBL GetProductBL()
        {
            return new ProductBL();
        }

        public IAdminBL GetAdminBL()
        {
            return new AdminBL();
        }

        public ITrainingService GetTrainingService()
        {
            return new TrainingService(); 
        }

        public ICartBL GetCartBL()
        {
            return new CartBL();
        }

    }
}
