using System;
using System.Collections.Generic;

namespace practice.Models
{
    public partial class Person
    {
        public Person()
        {
            Customer = new HashSet<Customer>();
            Address = new HashSet<Address>();
            CartItem = new HashSet<CartItem>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int AccessLevelId { get; set; }

        public virtual AccessLevel AccessLevel { get; set; }
        public virtual ICollection<Customer> Customer { get; set; }
        public virtual ICollection<Address> Address { get; set; }
        public virtual ICollection<CartItem> CartItem { get; set; }
    }
}
