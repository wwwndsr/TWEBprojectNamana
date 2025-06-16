using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.Entity.Migrations;
using webNamana.BusinessLogic.DBModel;
using webNamana.Domain.Entities.Product;

namespace webNamana.BusinessLogic.Core
{
    public class ProductApi
    {
        public bool AddProduct(ProductEntity product)
        {
            if (product == null)
                return false;

            try
            {
                using (var db = new ProductContext())
                {
                    db.Products.Add(product);
                    int affected = db.SaveChanges(); // возвращает количество затронутых строк

                    return affected > 0;
                }
            }
            catch (Exception ex)
            {
                try
                {
                    string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                    Directory.CreateDirectory(logDir);

                    string logPath = Path.Combine(logDir, "error_log.txt");

                    string errorMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Ошибка при добавлении продукта: {ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}";
                    File.AppendAllText(logPath, errorMessage);
                }
                catch
                {
                    // Не мешаем выполнению, если логирование не удалось
                }

                return false;
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
