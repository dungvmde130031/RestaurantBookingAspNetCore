using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppAspNetCore.Extensions;
using AppAspNetCore.Models;
using AppAspNetCore.ModelViews;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AppAspNetCore.Controllers
{
    public class ShoppingCart : Controller
    {
        private readonly Resbooking1Context _context;

        public INotyfService _notyfService { get; }

        public ShoppingCart(Resbooking1Context context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        public List<CartItem> Cart
        {
            get
            {
                var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
                if (cart == default(List<CartItem>))
                {
                    cart = new List<CartItem>();
                }
                return cart;
            }
        }

        [HttpPost]
        [Route("api/cart/add")]
        public IActionResult AddToCart(int mealId, int? amount)
        {
            List<CartItem> cart = Cart;

            try
            {
                // Add meal to cart
                CartItem item = Cart.SingleOrDefault(p => p.meal.MealId == mealId);
                if (item != null)
                {
                    if (amount.HasValue)
                    {
                        item.amount = amount.Value;
                    }
                    else
                    {
                        item.amount++;
                    }
                }
                else
                {
                    Meal meal1 = _context.Meals.SingleOrDefault(p => p.MealId == mealId);
                    item = new CartItem
                    {
                        amount = amount.HasValue ? amount.Value : 1,
                        meal = meal1
                    };
                    cart.Add(item);
                }

                // Save session
                HttpContext.Session.Set<List<CartItem>>("Cart", cart);
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        [Route("api/cart/remove")]
        public ActionResult Remove(int mealId)
        {
            try
            {
                List<CartItem> cart = Cart;

                CartItem item = cart.SingleOrDefault(p => p.meal.MealId == mealId);

                if (cart != null)
                {
                    cart.Remove(item);
                }

                // Save session
                HttpContext.Session.Set<List<CartItem>>("Cart", cart);
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [Route("cart.html", Name = "Cart")]
        public IActionResult Index()
        {
            var cartList = Cart;

            return View(Cart);
        }
    }
}