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

        public IProductService GetProductService()
        {
            return new ProductService();
        }

        public IAdminBL GetAdminBL()
        {
            return new AdminBL();
        }

        public ITrainingService GetTrainingService()
        {
            return new TrainingService(); // твоя реализация
        }


    }
}
