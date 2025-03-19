using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webNamana.Models
{
    public class CartItem
    {
        public int Id { get; set; } //уникальный идентификатор элемента корзины
        public int Quantity { get; set; } //количество товара
        public int ProductId { get; set; } //внешний ключ для товара
        public Product Product { get; set; } //навигационное свойство для товара
        public int CartId { get; set; } //внешний ключ для корзины
        public Cart Cart { get; set; } //навигационное свойство для корзины
    }
}