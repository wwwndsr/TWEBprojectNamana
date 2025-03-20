using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webNamana.Controllers;

namespace webNamana.Models
{
    public class OrderItem
    {
        public int Id { get; set; } //уникальный идентификатор элемента заказа
        public int Quantity { get; set; } //количество товара
        public decimal Price { get; set; } //цена товара
        public int ProductId { get; set; } //внешний ключ для товара
        public Product Product { get; set; } //навигационное свойство для товара
        public int OrderId { get; set; } //внешний ключ для заказа
        public Order Order { get; set; } //навигационное свойство для заказа
        //public decimal TotalPrice = Quantity * Price; 
    }
}