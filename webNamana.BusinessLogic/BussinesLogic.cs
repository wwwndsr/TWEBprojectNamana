using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webNamana.BusinessLogic.Core;
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
