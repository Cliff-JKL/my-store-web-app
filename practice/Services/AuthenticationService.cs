using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using practice.EF;
using practice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using practice.Services;
using practice.Models.ViewModels;
using practice.Repository;

namespace practice.Services
{
    public static class AuthenticationService
    {
        /// <summary>
        /// Synchronize cart items from Cookies and DataBase
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="controller"></param>
        /// <param name="person"></param>
        public static async Task<bool> SynchronizeShoppingCart(MystoreRepository rep, Controller controller, Person person)
        {
            MystoreRepository repository = rep;

            if (person == null)
                return false;

            Dictionary<int, int> databaseProducts = await GetDatabaseCartItems(rep, person);
            Dictionary<int, int> cookiesProducts = GetCookiesCartItems(controller);

            // updating DB cart items
            if (CookiesService.IsShoppingCartCookiesExist(controller))
            {
                foreach (var pair in cookiesProducts)
                {
                    int productId = pair.Key;
                    int productNumber = pair.Value;
                    CartItem cartItem;

                    cartItem = await repository.GetCartItem(person.Id, productId);
                    if (cartItem != null)
                    {
                        cartItem.Number += productNumber;
                        await repository.UpdateCartItem(cartItem);
                    }
                    else
                    {
                        cartItem = await repository.CreateCartItem(person.Id, productId, productNumber);
                    }

                    // save changes ???
                }
            }

            // unite full list
            foreach (var pair in databaseProducts)
            {
                if (cookiesProducts.ContainsKey(pair.Key))
                    cookiesProducts[pair.Key] += pair.Value;
                else
                    cookiesProducts.Add(pair.Key, pair.Value);
            }

            string cookiesCartData = "";
            int cookiesCartCost = 0;

            foreach (var pair in cookiesProducts)
            {
                Product product = await repository.GetProduct(pair.Key);
                cookiesCartData += "," + product.Id + "." + pair.Value;
                cookiesCartCost += product.Price * pair.Value;
            }

            if (!String.IsNullOrEmpty(cookiesCartData))
            {
                cookiesCartData = cookiesCartData.Substring(1);
                CookiesService.UpdateShoppingCartCookies(controller, cookiesCartCost.ToString(), cookiesCartData);
            }
            // await ???
            repository.SaveChanges();
            return true;
        }

        public async static Task<Dictionary<int, int>> GetDatabaseCartItems(MystoreRepository rep, Person person)
        {
            Dictionary<int, int> pairs = new Dictionary<int, int>();
            MystoreRepository repository = rep;

            var cartItems = await repository.GetCartItems(person.Id);
            if (cartItems == null)
                return new Dictionary<int, int>();

            foreach (CartItem item in cartItems)
            {
                pairs.Add(item.ProductId, item.Number);
            }

            return pairs;
        }

        public static Dictionary<int, int> GetCookiesCartItems(Controller controller)
        {
            if (!CookiesService.IsShoppingCartCookiesExist(controller))
                return new Dictionary<int, int>();

            Dictionary<int, int> pairs = new Dictionary<int, int>();

            string[] values = CookiesService.GetShoppingCartDataCookie(controller).Split(new char[] { ',' });

            for (int i = 0; i < values.Length; i++)
            {
                string[] data = values[i].Split(new char[] { '.' });
                int productId = Convert.ToInt32(data[0]);
                int productNumber = Convert.ToInt32(data[1]);

                pairs.Add(productId, productNumber);
            }

            return pairs;
        }
    }
}
