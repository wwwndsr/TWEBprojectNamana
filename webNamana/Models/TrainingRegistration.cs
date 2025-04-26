using System;
using System.ComponentModel.DataAnnotations;

namespace webNamana.Models
{
    public class TrainingRegistration
    {
        public int RegistrationId { get; set; }

        [Required(ErrorMessage = "Please enter your full name")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please select a training type")]
        [Display(Name = "Training Type")]
        public string TrainingType { get; set; }

        [Required(ErrorMessage = "Please select a date")]
        [Display(Name = "Preferred Date")]
        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; }

        [Required(ErrorMessage = "Please select a time")]
        [Display(Name = "Preferred Time")]
        [DataType(DataType.Time)]
        public TimeSpan TrainingTime { get; set; }

        [Display(Name = "Additional Notes")]
        public string AdditionalNotes { get; set; }

        public bool IsConfirmed { get; set; }
        public DateTime CreatedAt { get; set; }


    }

}
