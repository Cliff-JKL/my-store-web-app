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

namespace practice.Controllers
{
    public class ProductController : BaseController
    {
        public ProductController(mystoreContext context) : base(context) { }

        [HttpGet]
        [Route("Product/Index/{id}")]
        public async Task<ActionResult> Index(int id)
        {
            var product = await repository.GetProduct(id);
            if (product == null)
                return BadRequest();
            
            UpdateViewBag(product.Name);
            return View(product);
        }

        [Route("Product/AdminModeIndex/{id}")]
        public async Task<IActionResult> AdminModeIndex(int id)
        {
            if (IsAdmin() == false)
                return BadRequest();

            var product = await repository.GetProduct(id);
            if (product == null)
                return BadRequest();

            UpdateViewBag(product.Name);
            return View(product);
        }

        [HttpGet]
        [Route("Product/Create")]
        public async Task<IActionResult> Create()
        {
            var product = new Product();

            ViewBag.Subcategories = await repository.GetSubcategories();
            UpdateViewBag("Добавление товара");
            return View(product);
        }

        [HttpPost]
        [Route("Product/Create")]
        public async Task<IActionResult> Create(Product product, IFormFile uploadedImage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await repository.CreateProduct(product, uploadedImage);

            UpdateViewBag();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("Product/Delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var product = await repository.GetProduct(id);
            if (product == null)
                return BadRequest();

            UpdateViewBag("Удаление товара");
            return View(product);
        }

        [HttpPost]
        [Route("Product/Delete")]
        public async Task<IActionResult> Delete(Product product)
        {
            if (product == null)
                return BadRequest();

            await repository.DeleteProduct(product);

            UpdateViewBag();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("Product/Edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var product = await repository.GetProduct(id);

            ViewBag.Subcategories = await repository.GetSubcategories();
            UpdateViewBag("Редактирование товара");
            return View(product);
        }

        // FIX IT image editing doesn't work
        [HttpPost]
        [Route("Product/Edit")]
        public async Task<IActionResult> Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await repository.UpdateProduct(product);

            UpdateViewBag();
            return RedirectToAction("Index", "Home");
        }
    }
}
