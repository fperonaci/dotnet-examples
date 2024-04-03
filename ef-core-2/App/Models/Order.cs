using System.Data;

namespace ConsoleApp.Models
{
  public class Order
  {
    public int Id { get; set; }
    public DateTime OrderPlaced { get; set; }
    public DataSetDateTime? OrderFulfilled { get; set; }

    // this represents foreign key relationship
    // it'd get created anyways by EF (shadow property)
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public ICollection<OrderDetail> OrderDetails { get; set; } = null!;
  }
}
