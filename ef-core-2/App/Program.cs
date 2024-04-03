
using ConsoleApp.Data;
using ConsoleApp.Models;

using var context = new PizzaContext();

var veggieSpecial = new Product()
{
  Name = "Veggie Special Pizza",
  Price = 9.99M
};

context.Products.Add(veggieSpecial);

var deluxeMeat = new Product()
{
  Name = "Deluxe Meat Pizza",
  Price = 12.99M
};

context.Add(deluxeMeat); // equivalent to context.Products.Add() because it understands the type

context.SaveChanges();

foreach (var product in context.Products.Where(p => p.Price > 10.00M).OrderBy(p => p.Name))
{
  Console.WriteLine(product.Id);
  Console.WriteLine(product.Name);
  Console.WriteLine(product.Price);
}
