using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using webNamana.Domain.Enums;

namespace webNamana.Models
{
    public class UserDashboard
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public List<string> CartItems { get; set; }
        public URole Level { get; set; }
    }
}