using Google.Protobuf;

Console.WriteLine("Hello, World!");

var person1 = new Person()
{
    Age = 38,
    Name = "Franco",
    Nationality = Nationality.Italian,
    Vehicles = new Vehicles() {
        Cars = { "Bugatti", "Ferrari" },
        Bikes = { "Ducati", "Yamaha" }
        }
};

var person2 = new Person() { Age = 13, Name = "Mario" };
person2.Vehicles = new Vehicles();
person2.Vehicles.Cars.Add("Bugatti");
person2.Vehicles.Bikes.Add("Ducati");

var person3 = new Person() { Age = 38, Name = "Franco" };
person3.Vehicles = new Vehicles();
person3.Vehicles.Cars.Add("Ferrari");
person3.Vehicles.Bikes.Add("Yamaha");

// Writing multiple protos on same stream
// Non-repetead fields get overwritten, e.g. Age, Name
// Repetead fields get appended (also nested ones!), e.g. Vehicles, Cars and Bikes

using var stream = new MemoryStream();
person2.WriteTo(stream);
person3.WriteTo(stream);

stream.Position = 0;

var person4 = Person.Parser.ParseFrom(stream);

Console.WriteLine(person1);
Console.WriteLine(person4);

Console.WriteLine(Equals(person1, person4));
