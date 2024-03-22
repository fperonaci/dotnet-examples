using Confluent.Kafka;

var server = "localhost:9094";

if (args.Length == 0)
{
    var manager = new TopicsManager(server);
    var topics = manager.GetTopicsNames();
    Console.WriteLine($"Existing topics: {string.Join(", ", topics)}");
    // var groupIds = manager.GetConsumerGroupsIds();
    // Console.WriteLine($"Existing group ids: {string.Join(", ", groupIds)}");
    Console.WriteLine("Usage 1 : dotnet run -- produce <topic> <numPartitions>");
    Console.WriteLine("Usage 2 : dotnet run -- consume <topic> <groupId>");
    return;
}

var action = args[0];
var topic = args[1];

if (action == "produce")
{
    var numPartitions = int.Parse(args[2]);
    new TopicsManager(server).CreateTopicIfNotExists(topic, numPartitions);

    var config = new ProducerConfig()
    {
        BootstrapServers = server,
    };

    using var producer = new ProducerBuilder<string?, string?>(config).Build();

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
    var groupId = args.Length > 2 ? args[2] : Guid.NewGuid().ToString();

    if (!new TopicsManager(server).GetTopicsNames().Contains(topic))
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

    var consumer = new ConsumerBuilder<string, string>(config).Build();

    consumer.Subscribe(topic);

    Console.WriteLine($"Consuming messages with group id {groupId}..");

    while (true)
    {
        var result = consumer.Consume();
        var key = result.Message.Key;
        var value = result.Message.Value;
        var partition = result.Partition;
        var timestamp = result.Message.Timestamp;
        Console.WriteLine(key);
        Console.WriteLine(value);
        Console.WriteLine(partition);
        Console.WriteLine(timestamp);
    }
}

class TopicsManager
{
    private readonly AdminClientBuilder _builder;

    public TopicsManager(string bootstrapServers)
    {
        var config = new AdminClientConfig()
        {
            BootstrapServers = bootstrapServers
        };

        _builder = new AdminClientBuilder(config);
    }

    public IEnumerable<string> GetConsumerGroupsIds()
    {
        using var client = _builder.Build();

        return client.ListConsumerGroupsAsync().Result.Valid.Select(x => x.GroupId);
    }

    public IEnumerable<string> GetTopicsNames()
    {
        using var client = _builder.Build();

        return client.GetMetadata(TimeSpan.FromSeconds(60)).Topics.Select(x => x.Topic);
    }

    public void CreateTopicIfNotExists(string name, int numPartitions)
    {
        if (GetTopicsNames().Contains(name))
        {
            return;
        }

        using var client = _builder.Build();

        client.CreateTopicsAsync([new() { Name = name, NumPartitions = numPartitions }]).Wait();
    }

    public void DeleteTopicIfExists(string name)
    {
        if (!GetTopicsNames().Contains(name))
        {
            return;
        }

        using var client = _builder.Build();

        client.DeleteTopicsAsync([name]).Wait();
    }
}
