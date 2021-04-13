using System;
using System.Collections.Generic;

namespace practice.Models
{
    public partial class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int PersonId { get; set; }

        public int Number { get; set; }

        public virtual Person Person { get; set; }
        public virtual Product Product { get; set; }
    }
}
