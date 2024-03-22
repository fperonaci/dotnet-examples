using Confluent.Kafka;

var server = "localhost:9094";

var manager = new TopicsManager(server);

if (args.Length == 0)
{
    var topics = manager.GetTopicsNames();
    var groupIds = manager.GetConsumerGroupsIds();
    Console.WriteLine($"Existing topics: {string.Join(", ", topics)}");
    // Console.WriteLine($"Existing group ids: {string.Join(", ", groupIds)}");
    Console.WriteLine("Usage 1 : dotnet run -- produce <topic> <numPartitions>");
    Console.WriteLine("Usage 2 : dotnet run -- consume <topic> <groupId>");
    return;
}

var action = args[0];

if (action == "produce")
{
    var topic = args[1];

    var numPartitions = int.Parse(args[2]);

    manager.CreateTopicIfNotExists(topic, numPartitions);

    var config = new ProducerConfig()
    {
        BootstrapServers = server,
    };

    var builder = new ProducerBuilder<string?, string?>(config);

    using var producer = builder.Build();

    while (true)
    {
        Console.WriteLine("Enter key and value..");
        var message = new Message<string?, string?>
        {
            Key = Console.ReadLine(),
            Value = Console.ReadLine()
        };
        producer.Produce(topic, message);
        producer.Flush();
    }
}

if (action == "consume")
{
    var topic = args[1];

    var groupId = args.Length > 2 ? args[2] : Guid.NewGuid().ToString();

    if (!manager.GetTopicsNames().Contains(topic))
    {
        Console.WriteLine("Topic does not exist, shutting down");
        return;
    }

    var config = new ConsumerConfig()
    {
        BootstrapServers = server,
        GroupId = groupId,
        AutoOffsetReset = AutoOffsetReset.Earliest
    };

    var builder = new ConsumerBuilder<string, string>(config);

    var consumer = builder.Build();

    consumer.Subscribe(topic);

    Console.WriteLine($"Consuming messages with group id {groupId}..");

    while (true)
    {
        var result = consumer.Consume();
        var key = result.Message.Key;
        var value = result.Message.Value;
        Console.WriteLine(key);
        Console.WriteLine(value);
    }
}
