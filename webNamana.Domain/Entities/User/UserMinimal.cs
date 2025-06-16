using System;
using webNamana.Domain.Enums;

namespace webNamana.Domain.Entities.User
{
    [Serializable]
    public class UserMinimal
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public URole Level { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime RegisterTime { get; set; }
    }
}

