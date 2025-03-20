using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webNamana.BusinessLogic.Core;
using  webNamana.BusinessLogic.Interfaces;

namespace webNamana.BusinessLogic
{
   internal public class SessionBL : UserApi, ISession
    {
        private bool sessionActive = false;
        public bool Login(string username, string password)
        {
            if (ValidateUser(username, password))
            {
                sessionActive = true;
                return true;
            }
            return false;
        }

        public void Logout()
        {
            sessionActive = false;  

        }

        public bool IsSessionActive() { return sessionActive; }    
    }
}
