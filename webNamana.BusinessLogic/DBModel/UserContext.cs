using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webNamana.Domain.Entities.User
{
   public class UserContext : DbContext
    {
        public UserContext() :
            base("name=webNamana") // connectionstring name define in your web.config
        {
        }

        public virtual DbSet<UDbTable> Users { get; set; }
    }
}
    