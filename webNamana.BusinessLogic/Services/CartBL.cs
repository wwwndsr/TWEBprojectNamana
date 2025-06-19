using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webNamana.BusinessLogic.Core;
using webNamana.BusinessLogic.Interfaces;
using webNamana.Domain.Entities.Cart;

namespace webNamana.BusinessLogic.Services
{
    public class CartBL : CartApi, ICartBL
    {
        public new List<CartEntity> GetCartItems(string sessionId)
        {
            return base.GetCartItems(sessionId);
        }

        public new void AddToCart(string sessionId, CartEntity item)
        {
            base.AddToCart(sessionId, item);
        }

        public new void RemoveFromCart(string sessionId, int itemId)
        {
            base.RemoveFromCart(sessionId, itemId);
        }

        public new void ClearCart(string sessionId)
        {
            base.ClearCart(sessionId);
        }
    }
}
