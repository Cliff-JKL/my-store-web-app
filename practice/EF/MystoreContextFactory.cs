using System;
using Microsoft.EntityFrameworkCore;

namespace practice.EF
{
    public class MystoreContextFactory
    {
        public mystoreContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<mystoreContext>();

            optionsBuilder.UseMySql(connectionString);
            
            return new mystoreContext(optionsBuilder.Options);
        }
    }
}