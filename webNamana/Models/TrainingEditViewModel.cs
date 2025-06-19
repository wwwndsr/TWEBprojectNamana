using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace webNamana.Models
{
    public class TrainingEditViewModel : TrainingCreateViewModel
    {
        public int Id { get; set; }
    }
}