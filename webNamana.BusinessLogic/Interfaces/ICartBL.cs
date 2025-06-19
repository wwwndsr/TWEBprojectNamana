using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webNamana.Domain.Entities.Cart;

namespace webNamana.BusinessLogic.Interfaces
{
    public interface ICartBL
    {
        List<CartEntity> GetCartItems(string sessionId);
        void AddToCart(string sessionId, CartEntity item);
        void RemoveFromCart(string sessionId, int itemId);
        void ClearCart(string sessionId);
    }
}
