using App;

var server = "localhost:9094";

if (args.Length == 0)
{
    Console.WriteLine("Usage :");
    Console.WriteLine("dotnet run --project App -- list_topic");
    Console.WriteLine("dotnet run --project App -- create_topic <topic> <numPartitions>");
    Console.WriteLine("dotnet run --project App -- delete_topic <topic>");
    Console.WriteLine("dotnet run --project App -- produce <topic>");
    Console.WriteLine("dotnet run --project App -- consume <topic> [<groupId>]");
    return;
}

switch (args[0])
{
    case "list_topic":
        var topics = THelper.GetTopicsNames(server);
        var partitions = THelper.GetNumberOfPartitions(server);
        Console.WriteLine($"Existing topics: {string.Join(", ", topics)}");
        Console.WriteLine($"Partitions: {string.Join(", ", partitions)}");
        break;

    case "crate_topic":
        THelper.CreateTopicIfNotExists(server, args[1], int.Parse(args[2]));
        break;

    case "delete_topic":
        THelper.DeleteTopicIfExists(server, args[1]);
        break;

    case "produce":
        PHelper<string, int>.Produce(server, args[1]);
        break;

    case "consumer":
        CHelper<string, int>.Consume(server, args[1], args.Length > 2 ? args[2] : Guid.NewGuid().ToString());
        break;
}
