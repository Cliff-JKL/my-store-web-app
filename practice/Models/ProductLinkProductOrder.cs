using System;
using System.Collections.Generic;

namespace practice.Models
{
    public partial class ProductLinkProductOrder
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }

        public int Number { get; set; }

        public virtual ProductOrder Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
