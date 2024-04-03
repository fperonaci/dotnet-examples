using Microsoft.EntityFrameworkCore;

using PizzaDelivery.Models;

namespace PizzaDelivery.Data
{
    internal class JobContext : DbContext
    {
        public DbSet<Job> Jobs { get; set; }
        public DbSet<ProductInstance> ProductInstances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Job>().
                ToTable("jobs").
                HasKey(x => new { x.ProductInstanceId, x.Schema });

            modelBuilder.Entity<ProductInstance>().
                ToTable("product_instances").
                HasKey(x => new { x.Id });

            modelBuilder.Entity<ProductInstance>().
                HasMany(x => x.Jobs);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            var connectionString =
                "Host=localhost;" +
                "Database=pizzadb;" +
                "Port=5432;" +
                "Username=postgres;" +
                "Password=postgres;" +
                "Pooling=false;";

            optionBuilder.UseNpgsql(connectionString);
        }
    }
}
