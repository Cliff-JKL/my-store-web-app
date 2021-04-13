using System;
using System.Collections.Generic;

namespace practice.Models
{
    public partial class OrderStatus
    {
        public OrderStatus()
        {
            ProductOrder = new HashSet<ProductOrder>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ProductOrder> ProductOrder { get; set; }
    }
}
