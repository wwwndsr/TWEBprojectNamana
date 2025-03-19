using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webNamana.Models
{
    public class Cart
    {
        public int Id { get; set; } //уникальный идентификатор корзины
        public int UserId { get; set; } //внешний ключ для пользователя
        public User User { get; set; } //навигационное свойство для пользователя
        public List<CartItem> CartItem { get; set; } //список товаров в корзине
    }
}