using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webNamana.BusinessLogic.Core;
using webNamana.BusinessLogic.Interfaces;


namespace webNamana.BusinessLogic.Core
{
    class UserApi
    {
        public bool ValidateUser(string username, string password)
        {
            return username == "admin" && password == "1234";
        }

    }
}
