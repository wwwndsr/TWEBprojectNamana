using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webNamana.Domain.Entities.Training;

namespace webNamana.BusinessLogic.Interfaces
{
    public interface ITrainingService
    {
        TrainingEntity GetTrainingById(int id);
        List<TrainingEntity> GetAllTrainings();
        bool AddTraining(TrainingEntity newTraining);
        bool UpdateTraining(int id, TrainingEntity updatedTraining);
        bool DeleteTraining(int id);
    }
}
