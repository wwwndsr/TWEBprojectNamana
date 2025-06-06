using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webNamana.BusinessLogic.DBModel;
using webNamana.BusinessLogic.Core;
using webNamana.BusinessLogic.Interfaces;
using webNamana.Domain.Entities.Product;


namespace webNamana.BusinessLogic.Services
{
    public class ProductService : ProductApi, IProductService
    {
        public new ProductEntity GetProductById(int id)
        {
            return base.GetProductById(id);
        }

        public new List<ProductEntity> GetAllProducts()
        {
            return base.GetAllProducts();
        }

        public new bool AddProduct(ProductEntity newProduct)
        {
            return base.AddProduct(newProduct);
        }

        public new bool UpdateProduct(int id, ProductEntity updatedProduct)
        {
            return base.UpdateProduct(id, updatedProduct);
        }

        public new bool DeleteProduct(int id)
        {
            return base.DeleteProduct(id);
        }
    }
}
