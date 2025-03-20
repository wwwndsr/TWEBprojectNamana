using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webNamana.BusinessLogic.Core;
using webNamana.BusinessLogic.Interfaces;


namespace webNamana.BusinessLogic.Interfaces
{
    public interface ISession
    {
        bool Login (string username, string password);      
        bool Logout();  
        bool IsSessionActive(); 
    }
}
