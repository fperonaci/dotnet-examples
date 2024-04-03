using System.ComponentModel.DataAnnotations;

namespace ConsoleApp.Models
{
  public class Product
  {
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Color { get; set; }
  }
}
