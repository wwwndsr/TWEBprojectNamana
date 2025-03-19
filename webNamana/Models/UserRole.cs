using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webNamana.Models
{
    public class UserRole
    {
        public int Id { get; set; } //уникальный идентификатор роли
        public string Name { get; set; } //название роли (например, "Admin", "Customer", "Moderator")
        public string Description { get; set; } //описание роли (необязательно)
    }
}