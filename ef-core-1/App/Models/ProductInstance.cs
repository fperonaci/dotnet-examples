namespace PizzaDelivery.Models
{
    internal class ProductInstance
    {
        public int Id { get; set; }
        public ICollection<Job> Jobs { get; set; }
    }
}
