using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using practice.EF;
using practice.Models;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using practice.Services;

namespace practice.Controllers
{
    public class OrderController : BaseController
    {
        public OrderController(mystoreContext context) : base(context) { }

        [Route("Order/Details/{id}")]
        public async Task<IActionResult> Details([FromRoute] int id)
        {
            var values = await repository.GetOrder(id);
            var productOrder = values.Item1;
            if (productOrder == null)
                return BadRequest();

            UpdateViewBag("Заказ №" + productOrder.Id.ToString());
            ViewBag.orderProducts = values.Item2;
            return View(productOrder);
        }

        [HttpPost]
        [Route("Order/Create")]
        public async Task<IActionResult> Create()
        {
            Person person = GetCurrentPerson();

            if (person == null)
                return RedirectToAction("Info", "User");
            Customer currentCustomer = await repository.GetCustomer(person.Id);
            if (currentCustomer == null)
                return RedirectToAction("Info", "User");
            Address currentAddress = await repository.GetAddress(person.Id);
            if (currentAddress == null)
                return RedirectToAction("Info", "User");

            // get product from ShoppingCart
            if (CookiesService.IsShoppingCartCookiesExist(this))
            {
                string[] values = CookiesService.GetShoppingCartDataCookie(this).Split(new char[] { ',' });

                ProductOrder productOrder = await repository.CreateOrder(person, values);

                CookiesService.DeleteShoppingCartCookies(this);
                UpdateViewBag();
                return RedirectToAction("Info", "User");
            }
            else
            {
                UpdateViewBag();
                return RedirectToAction("Info", "ShoppingCart");
            }
        }
    }
}
