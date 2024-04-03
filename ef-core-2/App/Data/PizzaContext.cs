using ConsoleApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp.Data
{
  public class PizzaContext : DbContext
  {
    // DbSet is basically a table in the database
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<OrderDetail> OrderDetails { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseNpgsql(
        "Host=localhost;" +
        "Port=5432;" +
        "Database=pizza_db;" +
        "Username=postgres;" +
        "Password=postgres;");
    }
  }
}
