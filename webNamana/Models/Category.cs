using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webNamana.Models
{
    public class Category
    {
        public int Id { get; set; } //уникальный идентификатор категории
        public string Name { get; set; } //название категории
        public string Description { get; set; } //описание категории
        public List<Product> Products { get; set; } //список товаров в категории
    }
}