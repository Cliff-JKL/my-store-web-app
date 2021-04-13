using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.DependencyInjection;
using practice.EF;
using practice.Models;

namespace practice.Models.ViewModels
{
    public class User
    {
        public int Id { get; set; }

        public Person Person { get; set; }
        public Customer Customer { get; set; }
        public Address Address { get; set; }
        public List<ProductOrder> ProductOrders { get; set; } = new List<ProductOrder>();

        public string Login
        {
            get { return Person != null ? Person.Email : null; }
            set { if (Person != null) { Person.Email = value; } }
        }

        public string Password
        {
            get { return Person != null ? Person.Password : null; }
            set { if (Person != null) { Person.Password = value; } }
        }

        public bool IsCustomer
        {
            get { return Customer != null ? true : false; } set { }
        }

        public string Surname
        {
            get { return IsCustomer ? Customer.Surname : null; } set { }
        }

        public string Name
        {
            get { return IsCustomer ? Customer.Name : null; } set { }
        }

        public string Midname
        {
            get { return IsCustomer ? Customer.Midname : null; } set { }
        }

        public string Phone
        {
            get { return IsCustomer ? Customer.Phone : null; } set { }
        }

        public string City
        {
            get { return Address != null ? Address.City : null; } set { }
        }

        public string Street
        {
            get { return Address != null ? Address.Street : null; }
            set { }
        }

        public string Building
        {
            get { return Address != null ? Address.Building : null; }
            set { }
        }

        public User()
        {
        }
    }
}
