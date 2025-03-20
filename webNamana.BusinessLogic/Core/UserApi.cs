using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace webNamana.BusinessLogic.Core
{
    public class UserApi
    {
        public bool ValidateUser(string username, string password)
        {
            return username == "admin" && password == "1234";
        }
    }
}
