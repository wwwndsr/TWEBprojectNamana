using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webNamana.Domain.Entities.Training
{
    public class TrainingEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string TrainingName { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; } // Новый столбец: день недели

        [Required]
        public TimeSpan StartTime { get; set; } // Новый столбец: время начала тренировки
    }
}
