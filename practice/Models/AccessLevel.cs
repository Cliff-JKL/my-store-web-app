using System;
using System.Collections.Generic;

namespace practice.Models
{
    public partial class AccessLevel
    {
        public AccessLevel()
        {
            Person = new HashSet<Person>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Person> Person { get; set; }
    }
}
