using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using webNamana.Models;

namespace webNamana.Controllers
{
    public class CartPageController : Controller
    {
     //Хранение корзины в сессии
        private List<Cart> CartItems
        {
            get
            {
                if (Session["Cart"] == null)
                {
                    Session["Cart"] = new List<Cart>();
                }
                return (List<Cart>)Session["Cart"];
            }
            set
            {
                Session["Cart"] = value;
            }
        }

        // GET: CartPage
        public ActionResult CartPage()
        {
            return View(CartItems);
        }

        // Метод для добавления товара в корзину
        [HttpPost]
        public ActionResult AddToCart(int id, string productName, string productImage, decimal price)
        {
            var cartItem = CartItems.Find(item => item.ItemId == id);
            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                CartItems.Add(new Cart
                {
                    ItemId = id,
                    ProductName = productName,
                    ProductImage = productImage,
                    Price = price,
                    Quantity = 1,
                    InStock = true
                });
            }

            return RedirectToAction("CartPage");
        }

        // Метод для удаления товара из корзины
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var cartItem = CartItems.Find(item => item.ItemId == id);
            if (cartItem != null)
            {
                CartItems.Remove(cartItem);
            }

            return RedirectToAction("CartPage");
        }

    }

}