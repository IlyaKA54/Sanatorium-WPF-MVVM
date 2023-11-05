using Microsoft.EntityFrameworkCore;
using Sanatorium.Model.Entities;
using System.IO;

namespace Sanatorium.Model.Data
{
    public class SanatoriumContext : DbContext
    {
        public DbSet<TypeOfRoom> TypeOfRooms { get; set; } = null!;
        public DbSet<Room> Rooms { get; set; } = null!;
        public DbSet<TreatmentProgram> TreatmentPrograms { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<CustomerOrder> CustomerOrders { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=LAPTOP-6F52CNSF;Database=Sanatorium1;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}
