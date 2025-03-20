using webNamana.BusinessLogic.Interfaces;

namespace webNamana.BusinessLogic
{
    public class BusinessLogic
    {
        public ISession GetSessionBL()
        {
            return new SessionBL();  
        }          
    }
}
