using Microsoft.EntityFrameworkCore;


namespace CargoManagementAPi.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Cargo> Cargo { get; set; }
        public DbSet<CargoOrderDetails> CargoOrderDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Admin> Admin { get; set; }

        public DbSet<CargoType> CargoTypes { get; set; }


        public DbSet<City> Cities { get; set; }

        public DbSet<CargoStatus> cargoStatuses { get; set; }
    }
}
