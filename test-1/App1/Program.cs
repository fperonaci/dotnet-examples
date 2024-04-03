
// await CountAsYouAreGood();

// Console.WriteLine("Here comes the sun");

// static async Task CountAsYouAreGood()
// {
//     Console.WriteLine("Hello");
//     await Task.Delay(1000);
//     Console.WriteLine("My dear");
// }

using System.Diagnostics;

var sw = new Stopwatch();

sw.Start();

await Task.Run(() => Sleep(1));

sw.Stop();

Console.WriteLine(sw.Elapsed.TotalSeconds);

static void Sleep(int seconds)
{
    Thread.Sleep(seconds * 1000);
}