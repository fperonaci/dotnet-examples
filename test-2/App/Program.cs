
var type1 = Type.GetType(args[0]) ?? typeof(int);
var type2 = Type.GetType(args[1]) ?? typeof(int);

var x = typeof(PoliteGuy).GetMethod("SayHello");

var y = x?.MakeGenericMethod([type1, type2]);

y?.Invoke(null, null);

static class PoliteGuy
{
    public static void SayHello<T1, T2>()
    {
        Console.WriteLine(typeof(T1));
        Console.WriteLine(typeof(T2));
    }
}
