using System;
using System.Collections.Generic;

namespace practice.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductLinkProductOrder = new HashSet<ProductLinkProductOrder>();
            CartItem = new HashSet<CartItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Number { get; set; }
        public int CategoryId { get; set; }
        public int? ImageId { get; set; }

        public virtual Subcategory Category { get; set; }
        public virtual Image Image { get; set; }
        public virtual ICollection<ProductLinkProductOrder> ProductLinkProductOrder { get; set; }
        public virtual ICollection<CartItem> CartItem { get; set; }
    }
}
