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
using practice.Repository;

namespace practice.Controllers
{
    public class BaseController : Controller
    {
        private mystoreContext db;
        protected MystoreRepository repository;
        public BaseController(mystoreContext context)
        {
            db = context;
            repository = new MystoreRepository(db);
        }

        public List<Category> GetCategories()
        {
            return db.Category.ToList();
        }

        public string GetShoppingCartCost()
        {
            if (CookiesService.IsShoppingCartCookiesExist(this))
                return CookiesService.GetShoppingCartCostCookie(this);
            else
                return "0";
        }

        public string GetCustomerName()
        {
            if (CookiesService.IsPersonCookiesExist(this))
            {
                string login = CookiesService.GetLoginCookie(this);
                string password = CookiesService.GetPasswordCookie(this);

                Person person = db.Person.Where(p => p.Email == login).FirstOrDefault();
                person.Customer.Add(db.Customer.Where(c => c.PersonId == person.Id).FirstOrDefault());
                person.Address.Add(db.Address.Where(a => a.PersonId == person.Id).FirstOrDefault());

                if (person.Customer.First() != null)
                    return person.Customer.First().Name;
                else
                    return person.Email;
            }
            else
            {
                return "";
            }
        }

        protected Person GetCurrentPerson()
        {
            if (CookiesService.IsPersonCookiesExist(this))
            {
                string login = CookiesService.GetLoginCookie(this);
                string password = CookiesService.GetPasswordCookie(this);
                
                // password ?
                Person person = db.Person.Where(p => p.Email == login).FirstOrDefault();
                person.Customer.Add(db.Customer.Where(c => c.PersonId == person.Id).FirstOrDefault());
                person.Address.Add(db.Address.Where(a => a.PersonId == person.Id).FirstOrDefault());

                return person;
            }
            else
            {
                return null;
            }
        }

        protected bool IsEmailUnique(string email)
        {
            // return db.Person.Any(p => string.Compare(p.Email, email, true) == 0) ? false : true;
            return db.Person.Any(p => p.Email == email) ? false : true ;
        }

        protected bool IsAdmin()
        {
            if (CookiesService.IsPersonCookiesExist(this))
            {
                string login = CookiesService.GetLoginCookie(this);
                string password = CookiesService.GetPasswordCookie(this);

                Person person = db.Person.Where(p => p.Email == login).FirstOrDefault();
                if (person.AccessLevelId == 1)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public void UpdateViewBag(string pageTitle = "")
        {
            ViewBag.CustomerName = GetCustomerName();
            ViewBag.ShoppingCartCost = GetShoppingCartCost();
            ViewBag.Categories = GetCategories();
            ViewBag.Title = pageTitle;
        }
    }
}
