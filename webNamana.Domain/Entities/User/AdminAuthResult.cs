using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webNamana.Domain.Entities.User
{
    public class AdminAuthResult
    {
        public bool Status { get; set; }
        public string StatusMsg { get; set; }

        public List<UserMinimal> Users { get; set; }
        public UserMinimal User { get; set; }
    }
}

