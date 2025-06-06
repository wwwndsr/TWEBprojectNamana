using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webNamana.Domain.Entities.Product;

namespace webNamana.BusinessLogic.DBModel
{
    public class ProductContext : DbContext
    {
        public ProductContext() : base("name=webNamana") 
        {
        }

        public DbSet<ProductEntity> Products { get; set; }
    }
}