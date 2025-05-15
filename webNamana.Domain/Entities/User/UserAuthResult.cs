using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webNamana.Domain.Entities.User
{
    public class UserAuthResult
    {
        public bool Status { get; set; }
        public string StatusMsg { get; set; }
        public string StatusKey { get; set; }
    }

}
