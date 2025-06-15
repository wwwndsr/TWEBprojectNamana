using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webNamana.Domain.Entities.Product;

namespace webNamana.BusinessLogic.Interfaces
{
    public interface IProductService
    {
        ProductEntity GetProductById(int id);
        List<ProductEntity> GetAllProducts();
        bool AddProduct(ProductEntity newProduct);
        bool UpdateProduct(int id, ProductEntity updatedProduct);
        bool DeleteProduct(int id);
    }
}