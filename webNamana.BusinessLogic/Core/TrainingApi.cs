using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using webNamana.BusinessLogic.DBModel;
using webNamana.Domain.Entities.Training;
using System.Data.Entity.Migrations;

namespace webNamana.BusinessLogic.Core
{
    public class TrainingApi
    {
        public bool AddTraining(TrainingEntity training)
        {
            if (training == null)
                return false;

            try
            {
                using (var db = new TrainingContext())
                {
                    db.Trainings.Add(training);
                    return db.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
                try
                {
                    string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                    Directory.CreateDirectory(logDir);
                    string logPath = Path.Combine(logDir, "error_log.txt");
                    string errorMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Ошибка при добавлении тренировки: {ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}";
                    File.AppendAllText(logPath, errorMessage);
                }
                catch { }

                return false;
            }
        }

        public bool UpdateTraining(int id, TrainingEntity updated)
        {
            using (var db = new TrainingContext())
            {
                var existing = db.Trainings.FirstOrDefault(t => t.Id == id);
                if (existing == null) return false;

                existing.TrainingName = updated.TrainingName;
                existing.Description = updated.Description;
                existing.DurationMinutes = updated.DurationMinutes;
                existing.DifficultyLevel = updated.DifficultyLevel;

                db.Trainings.AddOrUpdate(existing);
                db.SaveChanges();
                return true;
            }
        }

        public bool DeleteTraining(int id)
        {
            using (var db = new TrainingContext())
            {
                var training = db.Trainings.FirstOrDefault(t => t.Id == id);
                if (training == null) return false;

                db.Trainings.Remove(training);
                db.SaveChanges();
                return true;
            }
        }

        public List<TrainingEntity> GetAllTrainings()
        {
            using (var db = new TrainingContext())
            {
                return db.Trainings.ToList();
            }
        }

        public TrainingEntity GetTrainingById(int id)
        {
            using (var db = new TrainingContext())
            {
                return db.Trainings.FirstOrDefault(t => t.Id == id);
            }
        }
    }
}
