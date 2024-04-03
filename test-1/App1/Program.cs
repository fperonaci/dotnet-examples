
var x = CountAsYouAreGood();

Console.WriteLine("Here comes the sun");

x.Wait();

static async Task CountAsYouAreGood()
{
    Console.WriteLine("Hello");
    await Task.Delay(1000);
    Console.WriteLine("My dear");
}
