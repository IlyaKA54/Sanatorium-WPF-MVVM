using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject3.Data
{
    public class SanatoriumContext : DbContext
    {

        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=LAPTOP-6F52CNSF;Database=Sanatorium4;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}
