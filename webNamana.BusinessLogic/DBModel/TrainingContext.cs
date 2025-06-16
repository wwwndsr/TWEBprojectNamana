using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using webNamana.Domain.Entities.Training;

namespace webNamana.BusinessLogic.DBModel
{
    public class TrainingContext : DbContext
    {
        public TrainingContext() : base("name=webNamana") { }

        public DbSet<TrainingEntity> Trainings { get; set; }
    }
}
