using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace webNamana.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Введите текущий пароль")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Введите новый пароль")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Подтвердите новый пароль")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmNewPassword { get; set; }
    }
}