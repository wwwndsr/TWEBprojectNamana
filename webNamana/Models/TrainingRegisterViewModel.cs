using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace webNamana.Models
{
    public class TrainingRegisterViewModel
    {
        public string TrainingType { get; set; }

        [Display(Name = "Date")]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Time")]
        public TimeSpan TrainingTime { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsConfirmed { get; set; }
    }
}