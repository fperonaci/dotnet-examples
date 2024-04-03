using Microsoft.EntityFrameworkCore;

using PizzaDelivery.Data;
using PizzaDelivery.Models;

var pi = new ProductInstance()
{
    Id = 1
};

var job = new Job()
{
    ProductInstance = pi,
    Schema = "Margherito"
};

var job2 = new Job()
{
    ProductInstance = pi,
    Schema = "Capricciosa"
};


// using (var context = new JobContext())
// {
//     context.Database.Migrate();
// }

// using (var context = new JobContext())
// {
//     context.Add(job);
// 
//     context.SaveChanges();
// }

// using (var context = new JobContext())
// {
//     context.Add(job2);
// 
//     context.SaveChanges();
// }

using var context = new JobContext();

var productInstance = // context.ProductInstances.FirstOrDefault(x => x.Id == 1);
context.ProductInstances.Include(x => x.Jobs).FirstOrDefault(x => x.Id == 1);

if (productInstance.Jobs is null)
    Console.WriteLine("I'm sorry joe !");

productInstance.Jobs.Add(job2);

context.SaveChanges();
