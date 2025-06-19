using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webNamana.Models
{
    public class TrainingListViewModel
    {
        public int Id { get; set; }
        public string TrainingName { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
    }
}