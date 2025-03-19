using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webNamana.Models
{
    public class Product
    {
        public int Id { get; set; } //уникальный идентификатор товара
        public string Name { get; set; } //название товара
        public string Description { get; set; } //описание товара
        public decimal UnitPrice { get; set; } //цена товара
        public int StockQuantity { get; set; } //количество товара на складе
        public int CategoryId { get; set; } //внешний ключ для категории
        public Category Category { get; set; } //навигационное свойство для категории
    }
}