using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webNamana.Models
{
    public class Order
    {
        public int Id { get; set; } //уникальный идентификатор заказа
        public DateTime OrderDate { get; set; } //дата заказа
        public decimal TotalAmount { get; set; } //общая сумма заказа
        public int UserId { get; set; } //внешний ключ для пользователя
        public User User { get; set; } //навигационное свойство для пользователя
        public List<OrderItem> OrderItems { get; set; } //список товаров на сайте
    }
}