using App;

var server = "localhost:9094";

if (args.Length != 0)
{
    switch (args[0])
    {
        case "list-topics":
            Console.WriteLine($"Existing topics (<name>, <numPartitions>):");
            THelper.GetTopicsNamesAndNumberOfPartitions(server)
            .ToList().ForEach(x => Console.WriteLine(x));
            return;

        case "create-topic":
            var numPartitions = args.Length > 2 ? int.Parse(args[2]) : 1;
            THelper.CreateTopicIfNotExists(server, args[1], numPartitions);
            return;

        case "delete-topic":
            THelper.DeleteTopicIfExists(server, args[1]);
            return;

        case "produce":
            PHelper<string, string>.Produce(server, args[1]);
            return;

        case "consume":
            var groupId = args.Length > 2 ? args[2] : Guid.NewGuid().ToString();
            CHelper<string, string>.Consume(server, args[1], groupId);
            return;
    }
}

Console.WriteLine("Usage:");
Console.WriteLine("dotnet run --project App -- list-topics");
Console.WriteLine("dotnet run --project App -- create-topic <topic> <numPartitions>");
Console.WriteLine("dotnet run --project App -- delete-topic <topic>");
Console.WriteLine("dotnet run --project App -- produce <topic>");
Console.WriteLine("dotnet run --project App -- consume <topic> [<groupId>]");
