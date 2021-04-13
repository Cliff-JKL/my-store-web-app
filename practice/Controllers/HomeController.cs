using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using practice.EF;
using practice.Models;
using System.Collections.Generic;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using practice.Repository;

namespace practice.Controllers
{
    //[Route("api/[controller]")]
    public class HomeController : BaseController
    {
        public HomeController(mystoreContext context) : base(context) { }

        [Route("")]
        [Route("Home")]
        [Route("Home/{category?}")]
        [Route("Home/Index/{category?}")]
        // [Route("Home")]
        // [Route("Home/Index/{category?}")]
        [HttpGet]
        public async Task<ActionResult> Index(string category)
        {
            if (IsAdmin())
                return RedirectToAction("AdminModeIndex", new { category = category});

            string pageTitle = "";
            var products = await repository.GetProducts(category);

            if (!String.IsNullOrEmpty(category))
            {
                var currentCategory = await repository.GetCategory(category);
                if (currentCategory != null)
                    pageTitle = currentCategory.Name;
            }

            UpdateViewBag(pageTitle);
            return View(products);
        }

        [Route("Home/AdminModeIndex")]
        [Route("Home/AdminModeIndex/{category?}")]
        [HttpGet]
        public async Task<IActionResult> AdminModeIndex(string category)
        {
            if (!IsAdmin())
                return BadRequest();

            string pageTitle = "";
            var products = await repository.GetProducts(category);

            if (!String.IsNullOrEmpty(category))
            {
                var currentCategory = await repository.GetCategory(category);
                if (currentCategory != null)
                    pageTitle = currentCategory.Name;
            }

            UpdateViewBag(pageTitle);
            return View(products);
        }

        [Route("Home/order-history")]
        //[Route("Home/Order-history")]
        //[Route("order-history")]
        [HttpGet]
        public async Task<IActionResult> OrderHistory()
        {
            Person person = GetCurrentPerson();
            string pageTitle = "";

            var orders = await repository.GetOrders(person);

            if (repository.PersonIsAdmin(person))
                pageTitle = "Список заказов";
            else
                pageTitle = "Мои заказы";

            UpdateViewBag(pageTitle);
            return View(orders);
        }
    }
}
