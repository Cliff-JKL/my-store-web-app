using System;
using System.Collections.Generic;

namespace practice.Models
{
    public partial class Image
    {
        public Image()
        {
            Product = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Filename { get; set; }
        public string Title { get; set; }
        public byte[] ImageData { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
