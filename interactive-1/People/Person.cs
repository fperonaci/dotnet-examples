namespace People;

public static class Helper
{
  public static void SayHello() => Console.WriteLine("Hello");
}

public record Person(string Name, int Age);
