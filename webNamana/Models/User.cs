using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webNamana.Models
{
	public class User
	{
		public int Id { get; set; } //уникальный идентификатор пользователя
        public string Username { get; set; } //имя пользователя
        public string PasswordHash { get; set; } //хэш пароля
        public string Email { get; set; } //электронная почта
        public string UserRole { get; set; } //роль пользователя (например, "Admin", "Customer")
        public List<Order> Orders { get; set; } //список заказов пользователя
    }
}