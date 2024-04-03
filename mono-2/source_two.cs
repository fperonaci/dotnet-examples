using System;

namespace HelloWorldApp
{
  class A
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");
      var b = new B();
      b.startMusic();
      Console.ReadKey();
    }
  }
}
