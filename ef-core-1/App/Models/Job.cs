namespace PizzaDelivery.Models
{
    internal class Job
    {
        public int ProductInstanceId { get; set; }
        public ProductInstance ProductInstance { get; set; }
        public string Schema { get; set; }
    }
}