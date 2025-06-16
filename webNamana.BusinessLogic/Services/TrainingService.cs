using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webNamana.BusinessLogic.Core;
using webNamana.BusinessLogic.Interfaces;
using webNamana.Domain.Entities.Training;

namespace webNamana.BusinessLogic.Services
{
    public class TrainingService : TrainingApi, ITrainingService
    {
        public new TrainingEntity GetTrainingById(int id)
        {
            return base.GetTrainingById(id);
        }

        public new List<TrainingEntity> GetAllTrainings()
        {
            return base.GetAllTrainings();
        }

        public new bool AddTraining(TrainingEntity newTraining)
        {
            return base.AddTraining(newTraining);
        }

        public new bool UpdateTraining(int id, TrainingEntity updatedTraining)
        {
            return base.UpdateTraining(id, updatedTraining);
        }

        public new bool DeleteTraining(int id)
        {
            return base.DeleteTraining(id);
        }
    }
}
