using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webNamana.Domain.Enums;

namespace webNamana.Models
{
    public class ChangeRoleViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public string CurrentRole { get; set; }
        public string NewRole { get; set; }

        // список доступных ролей для <select>
        public List<string> AvailableRoles { get; set; }
    }
}
