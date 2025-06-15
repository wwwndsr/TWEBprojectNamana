using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity.Migrations;

using webNamana.BusinessLogic.DBModel;
using webNamana.Domain.Entities.Product;

namespace webNamana.BusinessLogic.Core
{
    public class ProductApi
    {
        public bool AddProduct(ProductEntity product)
        {
            using (var db = new ProductContext())
            {
                db.Products.Add(product);
                db.SaveChanges();
                return true;
            }
        }

        public bool UpdateProduct(int id, ProductEntity updated)
        {
            using (var db = new ProductContext())
            {
                var existing = db.Products.FirstOrDefault(p => p.Id == id);
                if (existing == null) return false;

                existing.ProductName = updated.ProductName;
                existing.Description = updated.Description;
                existing.Price = updated.Price;
                existing.ProductImage = updated.ProductImage;

                db.Products.AddOrUpdate(existing);
                db.SaveChanges();
                return true;
            }
        }

        public bool DeleteProduct(int id)
        {
            using (var db = new ProductContext())
            {
                var product = db.Products.FirstOrDefault(p => p.Id == id);
                if (product == null) return false;

                db.Products.Remove(product);
                db.SaveChanges();
                return true;
            }
        }

        public List<ProductEntity> GetAllProducts()
        {
            using (var db = new ProductContext())
            {
                return db.Products.ToList();
            }
        }

        public ProductEntity GetProductById(int id)
        {
            using (var db = new ProductContext())
            {
                return db.Products.FirstOrDefault(p => p.Id == id);
            }
        }
    }

}
