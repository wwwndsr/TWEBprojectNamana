using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace webNamana.Domain.Entities.User
{
   public  class ULoginData
    {
        public string Credential { get; set; }
        public string Password { get; set; }
        public string LoginIp { get; set; }
        public DateTime LoginDateTime { get; set; }
    }
}
