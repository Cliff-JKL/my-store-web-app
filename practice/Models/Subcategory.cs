using System;
using System.Collections.Generic;

namespace practice.Models
{
    public partial class Subcategory
    {
        public Subcategory()
        {
            Product = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Product> Product { get; set; }
    }
}
