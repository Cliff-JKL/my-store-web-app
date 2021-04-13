using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using practice.EF;
using practice.Models;

namespace practice.Models.ViewModels
{
    public class ShoppingCart
    {
        public string ShoppingCartId { get; set; }
        public Dictionary<Product, int> ProductCountPairs { get; set; } = new Dictionary<Product, int>();
        public int Cost { get; set; } = 0;

        public ShoppingCart()
        {
            
        }

        public void AddToCart(Product product, int count=1)
        {
            if (ProductCountPairs.ContainsKey(product))
            {
                ProductCountPairs[product] += count;
            }
            else
            {
                ProductCountPairs.Add(product, count);
            }

            Cost += product.Price * count;
        }

        public static void Synchronize()
        {

        }
    }
}
