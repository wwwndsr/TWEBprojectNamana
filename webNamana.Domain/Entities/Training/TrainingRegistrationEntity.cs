using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webNamana.Domain.Entities.Training
{
    public class TrainingRegistrationEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } // из SessionHelper.User.Username

        [Required]
        public string TrainingType { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        [Required]
        public TimeSpan TrainingTime { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsConfirmed { get; set; }
    }
}
