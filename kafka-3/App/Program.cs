var server = "localhost:9094";

if (args.Length == 0)
{
    var topics = new TopicsManager(server).GetTopicsNames();
    Console.WriteLine($"Existing topics: {string.Join(", ", topics)}");
    Console.WriteLine("Usage 1 : dotnet run -- produce <topic> <numPartitions>");
    Console.WriteLine("Usage 2 : dotnet run -- consume <topic> <groupId>");
    return;
}

var action = args[0];
var topic = args[1];

if (action == "produce")
{
    Helper<string, int>.Produce(server, topic, args.Length > 2 ? int.Parse(args[2]) : 1);
}

if (action == "consume")
{
    Helper<string, int>.Consume(server, topic, args.Length > 2 ? args[2] : null);
}
