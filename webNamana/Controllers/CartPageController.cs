using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using webNamana.Models;
using webNamana.BusinessLogic;
using webNamana.BusinessLogic.Interfaces;
using webNamana.Domain.Entities.Cart;
using System.Linq;

namespace webNamana.Controllers
{
    public class CartPageController : Controller
    {
        private readonly ICartBL _cartService;

        public CartPageController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _cartService = bl.GetCartBL(); // как с продуктами
        }

        private string GetSessionId()
        {
            return Session.SessionID ?? Guid.NewGuid().ToString();
        }

        // GET: /CartPage
        public ActionResult CartPage()
        {
            string sessionId = GetSessionId();
            var cartEntities = _cartService.GetCartItems(sessionId);

            var cartViewModels = cartEntities.Select(item => new CartViewModel
            {
                ItemId = item.ItemId,
                ProductName = item.ProductName,
                ProductImage = item.ProductImage,
                Price = item.Price,
                Quantity = item.Quantity,
                Size = item.Size,         // Добавляем размер
                InStock = item.InStock
            }).ToList();

            return View(cartViewModels);
        }



        // POST: /CartPage/AddToCart
        [HttpPost]
        public ActionResult AddToCart(int id, string productName, string productImage, decimal price, string size)
        {
            string sessionId = GetSessionId();

            var newItem = new CartEntity
            {
                ItemId = id,
                ProductName = productName,
                ProductImage = productImage,
                Price = price,
                Quantity = 1,
                Size = size,
                InStock = true
            };

            _cartService.AddToCart(sessionId, newItem);
            return RedirectToAction("CartPage");
        }

        // POST: /CartPage/RemoveFromCart
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            string sessionId = GetSessionId();
            _cartService.RemoveFromCart(sessionId, id);
            return RedirectToAction("CartPage");
        }

        // POST: /CartPage/Clear
        [HttpPost]
        public ActionResult ClearCart()
        {
            string sessionId = GetSessionId();
            _cartService.ClearCart(sessionId);
            return RedirectToAction("CartPage");
        }
    }
}
