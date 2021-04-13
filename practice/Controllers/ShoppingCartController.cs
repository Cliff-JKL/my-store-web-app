using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using practice.EF;
using practice.Models;
using System.Collections.Generic;
using practice.Models.ViewModels;
using practice.Services;

namespace practice.Controllers
{
    public class ShoppingCartController : BaseController
    {
        public ShoppingCart Cart { get; set; }
        public ShoppingCartController(mystoreContext context) : base(context)
        {
            Cart = new ShoppingCart();
        }

        [Route("ShoppingCart/Info")]
        [HttpGet]
        public async Task<IActionResult> Info()
        {
            if (CookiesService.IsShoppingCartCookiesExist(this))
            {
                string[] values = CookiesService.GetShoppingCartDataCookie(this).Split(new char[] { ',' });

                for (int i = 0; i < values.Length; i++)
                {
                    string[] data = values[i].Split(new char[] { '.' });

                    Product product = await repository.GetProduct(Convert.ToInt32(data[0]));
                    Cart.AddToCart(product, Convert.ToInt32(data[1]));
                }
            }

            UpdateViewBag("Корзина");
            return View(Cart);
        }

        [HttpPost]
        [Route("ShoppingCart/AddToCart/{id}")]
        public async Task<IActionResult> AddToCart([FromRoute] int id)
        {
            var product = await repository.GetProduct(id);
            int number = 1;

            // get cart data from cookies
            Dictionary<int, int> cookiesProducts = AuthenticationService.GetCookiesCartItems(this);

            if (cookiesProducts.ContainsKey(product.Id))
                cookiesProducts[product.Id] += number;
            else
                cookiesProducts.Add(product.Id, number);

            string cartCost = (Convert.ToInt32(CookiesService.GetShoppingCartCostCookie(this)) + product.Price * number).ToString();
            string cartData = "";
            foreach (var pair in cookiesProducts)
            {
                cartData += "," + pair.Key + "." + pair.Value;
            }
            cartData = cartData.Substring(1);

            CookiesService.UpdateShoppingCartCookies(this, cartCost, cartData);

            var person = GetCurrentPerson();
            if (person != null)
            {
                CartItem cartItem = await repository.GetCartItem(person.Id, product.Id);

                if (cartItem != null)
                {
                    cartItem.Number++;
                    await repository.UpdateCartItem(cartItem);
                }
                else
                {
                    cartItem = await repository.CreateCartItem(person.Id, product.Id, number);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Route("ShoppingCart/DecreaseNumberOfCartItem/{id}")]
        public async Task<IActionResult> DecreaseNumberOfCartItem([FromRoute] int id)
        {
            var person = GetCurrentPerson();
            if (person != null)
            {
                var product = await repository.GetProduct(id);
                Dictionary<int, int> pairs = await AuthenticationService.GetDatabaseCartItems(repository, person);
                CartItem cartItem = await repository.GetCartItem(person.Id, product.Id);

                if (cartItem.Number > 1)
                    cartItem.Number--;
                else
                    await repository.DeleteCartItem(cartItem);

                await repository.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Route("ShoppingCart/RemoveFromCart/{id}")]
        public async Task<IActionResult> RemoveFromCart([FromRoute] int id)
        {
            var person = GetCurrentPerson();
            if (person != null)
            {
                var product = await repository.GetProduct(id);
                Dictionary<int, int> pairs = await AuthenticationService.GetDatabaseCartItems(repository, person);
                CartItem cartItem = await repository.GetCartItem(person.Id, product.Id);
                await repository.DeleteCartItem(cartItem);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
