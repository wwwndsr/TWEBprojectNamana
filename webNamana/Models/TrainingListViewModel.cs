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

        // Вычисляемое свойство 
        public DateTime StartDateTime
        {
            get
            {
                DateTime today = DateTime.Today;
                int daysUntil = ((int)DayOfWeek - (int)today.DayOfWeek + 7) % 7;
                DateTime date = today.AddDays(daysUntil);

                return date.Date + StartTime;
            }
        }
    }
}

