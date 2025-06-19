using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace webNamana.Models
{
    public class TrainingCreateViewModel
    {

        [Required]
        public string TrainingName { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        public TimeSpan StartTime { get; set; }


    }
}