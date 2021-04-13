using System;
using System.Collections.Generic;

namespace practice.Models
{
    public partial class Customer
    {
        public Customer()
        {
            ProductOrder = new HashSet<ProductOrder>();
        }

        public int Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Midname { get; set; }
        public string Phone { get; set; }
        public int PersonId { get; set; }

        public virtual Person Person { get; set; }
        public virtual ICollection<ProductOrder> ProductOrder { get; set; }
    }
}
