using Confluent.Kafka;

public static partial class Helper<TKey, TValue>
{
    public static void Consume(string server, string topic, string groupId, AutoOffsetReset offsetReset)
    {
        var config = new ConsumerConfig()
        {
            BootstrapServers = server,
            GroupId = groupId,
            AutoOffsetReset = offsetReset
        };

        var consumer = new ConsumerBuilder<TKey, TValue>(config).Build();

        consumer.Subscribe(topic);

        Console.WriteLine($"Consuming messages with group id {groupId}..");

        Console.CancelKeyPress += delegate
        {
            Console.WriteLine("I'm being shut down, good bye!");
        };

        while (true)
        {
            var result = consumer.Consume();
            var key = result.Message.Key;
            var value = result.Message.Value;
            var partition = result.Partition;
            var timestamp = result.Message.Timestamp;
            Console.WriteLine($"{key} {value} {partition} {timestamp.UtcDateTime:HH:mm:ss}");
        }
    }

    public static void Consume(string server, string topic) =>
        Consume(server, topic, Guid.NewGuid().ToString(), AutoOffsetReset.Earliest);

    public static void Consume(string server, string topic, string? groupId) =>
        Consume(server, topic, groupId ?? Guid.NewGuid().ToString(), AutoOffsetReset.Earliest);
}
