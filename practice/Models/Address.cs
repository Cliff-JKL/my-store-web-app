using System;
using System.Collections.Generic;

namespace practice.Models
{
    public partial class Address
    {
        public Address()
        {
            ProductOrder = new HashSet<ProductOrder>();
        }

        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int PersonId { get; set; }
        public string Building { get; set; }

        public virtual Person Person { get; set; }
        public virtual ICollection<ProductOrder> ProductOrder { get; set; }
    }
}
