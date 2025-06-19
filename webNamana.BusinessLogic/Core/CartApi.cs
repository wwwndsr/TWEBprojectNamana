using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using webNamana.Domain.Entities.Cart;

namespace webNamana.BusinessLogic.Core
{
    public class CartApi
    {
        private const string CartKeyPrefix = "Cart_";

        private List<CartEntity> GetSessionCart(string sessionId)
        {
            var key = CartKeyPrefix + sessionId;

            if (HttpContext.Current.Session[key] == null)
            {
                HttpContext.Current.Session[key] = new List<CartEntity>();
            }

            return (List<CartEntity>)HttpContext.Current.Session[key];
        }

        public List<CartEntity> GetCartItems(string sessionId)
        {
            return GetSessionCart(sessionId);
        }

        public void AddToCart(string sessionId, CartEntity newItem)
        {
            var cart = GetSessionCart(sessionId);
            var existingItem = cart.FirstOrDefault(c => c.ItemId == newItem.ItemId && c.Size == newItem.Size);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(newItem);
            }
        }

        public void RemoveFromCart(string sessionId, int itemId)
        {
            var cart = GetSessionCart(sessionId);
            var item = cart.FirstOrDefault(c => c.ItemId == itemId);
            if (item != null)
            {
                cart.Remove(item);
            }
        }

        public void ClearCart(string sessionId)
        {
            var key = CartKeyPrefix + sessionId;
            HttpContext.Current.Session[key] = new List<CartEntity>();
        }
    }
}
