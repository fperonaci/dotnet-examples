
using System.Text;
using System.Text.Json;

var x = new Animal() { Name = "Tobia", Age = 13 };

var y = JsonSerializer.Serialize(x);

Console.WriteLine(y);

var z = Encoding.UTF8.GetBytes(y);

z.ToList<byte>().ForEach(x => Console.WriteLine(x));

class Animal
{
    public string? Name { get; set; }
    public int Age { get; set; }
}
