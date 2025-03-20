using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webNamana.Models
{
    public class Register
    {
        //[Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } //имя пользователя

        //[Required(ErrorMessage = "Email is required.")]
        //[EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } //электронная почта пользователя

        //[Required(ErrorMessage = "Password is required.")]
        //[DataType(DataType.Password)]
        public string Password { get; set; } //пароль пользователя

       // [Required(ErrorMessage = "Confirm Password is required.")]
       // [DataType(DataType.Password)]
       // [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } //подтверждение пароля

        // Дополнительные поля (необязательно)
        public string FirstName { get; set; } //имя
        public string LastName { get; set; } //фамилия
        public string PhoneNumber { get; set; } //номер телефона
    }
}