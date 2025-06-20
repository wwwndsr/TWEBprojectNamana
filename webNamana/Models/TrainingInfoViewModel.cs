using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webNamana.Models
{
    public class TrainingInfoViewModel
    {
        public string TrainingType { get; set; }
        public DateTime RegistrationDate { get; set; }
        public TimeSpan TrainingTime { get; set; }
        public bool IsConfirmed { get; set; }
    }

}