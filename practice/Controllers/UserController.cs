using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using practice.EF;
using practice.Models;
using practice.Models.ViewModels;
using practice.Services;

namespace practice.Controllers
{
    public class UserController : BaseController
    {
        public UserController(mystoreContext context) : base(context) { }

        [Route("User")]
        [Route("User/Info")]
        [HttpGet]
        public async Task<IActionResult> Info()
        {
            if (CookiesService.IsPersonCookiesExist(this))
            {
                string email = CookiesService.GetLoginCookie(this);
                string password = CookiesService.GetPasswordCookie(this);
                User user = new User();
                Person person = await repository.GetPerson(email, password);
                if (person == null)
                    return BadRequest();
                Customer customer = await repository.GetCustomer(person.Id);
                Address address = await repository.GetAddress(person.Id);

                user = new User()
                {
                    Person = person,
                    Customer = customer,
                    Address = address
                };

                // tmp
                if (customer != null)
                {
                    user.ProductOrders = await repository.GetOrders(person);
                }

                UpdateViewBag();
                return View(user);
            }
            else
            {
                UpdateViewBag("Аутентификация");
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        [Route("User/Registration")]
        public async Task<IActionResult> Registration()
        {
            Person person = new Person();

            UpdateViewBag("Регистрация");
            return View(person);
        }


        [HttpPost]
        [Route("User/Registration")]
        public async Task<IActionResult> Registration(Person person)
        {
            if (String.IsNullOrWhiteSpace(person.Email) || String.IsNullOrWhiteSpace(person.Password) || !IsEmailUnique(person.Email))
                return RedirectToAction("Registration");

            person.AccessLevelId = 2;

            await repository.CreatePerson(person);

            CookiesService.UpdatePersonCookies(this, person.Email, person.Password);

            UpdateViewBag("Регистрация");
            return RedirectToAction("Index", "Home");
        }

        [Route("User/Logout")]
        public async Task<IActionResult> Logout(User user)
        {
            user = new User();

            CookiesService.DeletePersonCookies(this);

            CookiesService.DeleteShoppingCartCookies(this);

            UpdateViewBag("Выход");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("User/Edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            string email = CookiesService.GetLoginCookie(this);
            string password = CookiesService.GetPasswordCookie(this);

            Person person = await repository.GetPerson(email, password);
            if (person.Id != id)
                return BadRequest();

            UpdateViewBag("Редактор профиля");
            return View(person);
        }

        [HttpPost]
        [Route("User/Edit")]
        public async Task<IActionResult> Edit(Person person)
        {
            await repository.UpdatePerson(person);

            CookiesService.UpdatePersonCookies(this, person.Email, person.Password);

            UpdateViewBag("Обновление");
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        [Route("User/EditCustomer/{id}")]
        public async Task<IActionResult> EditCustomer([FromRoute] int id)
        {
            Customer customer = await repository.GetCustomer(id);
            if (customer == null)
            {
                customer = new Customer();
                customer.PersonId = id;
            }

            UpdateViewBag("Редактор профиля");
            return View(customer);
        }

        [HttpPost]
        [Route("User/EditCustomer")]
        public async Task<IActionResult> EditCustomer(Customer customer)
        {
            await repository.UpdateCustomer(customer);

            UpdateViewBag("Обновление");
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        [Route("User/EditAddress/{id}")]
        public async Task<IActionResult> EditAddress([FromRoute] int id)
        {
            Address address = await repository.GetAddress(id);
            if (address == null)
            {
                address = new Address();
                address.PersonId = id;
            }

            UpdateViewBag("Редактор профиля");
            return View(address);
        }

        [HttpPost]
        [Route("User/EditAddress")]
        public async Task<IActionResult> EditAddress(Address address)
        {
            await repository.UpdateAddress(address);

            UpdateViewBag("Обновление");
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        [Route("User/Login")]
        public async Task<IActionResult> Login()
        {
            Person person = new Person();

            UpdateViewBag("Аутентификация");
            return View(person);
        }

        [HttpPost]
        [Route("User/Login")]
        public async Task<IActionResult> Login(Person person)
        {
            // if (!ModelState.IsValid())
            //    ModelState.
            Person existingPerson = await repository.GetPerson(person.Email, person.Password);
            UpdateViewBag("Аутентификация");
            if (existingPerson != null)
            {
                // успешная аутентификация
                CookiesService.UpdatePersonCookies(this, existingPerson.Email, existingPerson.Password);
                bool ready = await AuthenticationService.SynchronizeShoppingCart(repository, this, existingPerson);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                // поля не совпадают
                return View(person);
            }
        }
    }
}
