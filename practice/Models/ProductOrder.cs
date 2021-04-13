using System;
using System.Collections.Generic;

namespace practice.Models
{
    public partial class ProductOrder
    {
        public ProductOrder()
        {
            ProductLinkProductOrder = new HashSet<ProductLinkProductOrder>();
        }

        public int Id { get; set; }
        public int Cost { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public int AddressId { get; set; }
        public int StatusId { get; set; }

        public virtual Address Address { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual OrderStatus Status { get; set; }
        public virtual ICollection<ProductLinkProductOrder> ProductLinkProductOrder { get; set; }
    }
}
