using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webNamana.Domain.Entities.User;

namespace webNamana.BusinessLogic.DBModel
{
    public class UserContext : DbContext
    {
        public UserContext() : base("name=webNamana")
        {
        }

        public DbSet<UDbTable> Users { get; set; }

       
    }
}