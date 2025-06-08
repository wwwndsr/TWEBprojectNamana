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
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public int DurationMinutes { get; set; }  // длительность тренировки

        [Required]
        [StringLength(50)]
        public string DifficultyLevel { get; set; } // например, "Beginner", "Intermediate", "Advanced"
    }
}
