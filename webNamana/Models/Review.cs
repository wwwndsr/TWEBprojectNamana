using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webNamana.Models
{
    public class Class1
    {
        public int Id { get; set; } //уникальный идентификатор отзыва
        public string Comment { get; set; } //текст отзыва
        public int Rating { get; set; } //рейтинг (от 1 до 10)
        public DateTime ReviewDate { get; set; } //дата отзыва
        public int ProductId { get; set; } //внешний ключ для товара
        public Product Product { get; set; } //навигационное свойство для товара
        public int UserId { get; set; } //внешний ключ для пользователя
        public User User { get; set; } //навигационное свойство для пользователя
    }
}