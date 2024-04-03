var x = SaySimpleWay(1);

// x.Wait();

var y = SaySimpleWay(2);

static async Task SaySimpleWay(int power)
{
    for (int i = 0; i < 5; i++)
    {
        Console.WriteLine($"In simple {i} to the {power} terms {Math.Pow(i, power)}");
        await Task.Delay(1000);
    }
}