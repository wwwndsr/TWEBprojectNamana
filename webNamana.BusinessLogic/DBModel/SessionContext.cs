using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webNamana.Domain.Entities.User;

namespace webNamana.BusinessLogic.DBModel
{
    public class SessionContext : DbContext
    {
        public SessionContext() : base("name=webNamana")
        {
        }

        public virtual DbSet<Session> Sessions { get; set; }
    }
}