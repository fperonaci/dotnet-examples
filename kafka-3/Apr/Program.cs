using Confluent.Kafka;

using Streamiz.Kafka.Net.SerDes;
using Streamiz.Kafka.Net.State;

byte[] x = [0, 1, 1];

Console.WriteLine(x[0]);
Console.WriteLine(x[1]);
Console.WriteLine(x[2]);

Console.WriteLine(x.Length);

return;

var server = "localhost:9094";
var topic = args[0];
var groupId = Guid.NewGuid().ToString();

var config = new ConsumerConfig()
{
    BootstrapServers = server,
    GroupId = groupId,
    AutoOffsetReset = AutoOffsetReset.Earliest
};

var consumer = new ConsumerBuilder<Windowed<string>, string>(config)
    .SetKeyDeserializer(new WindowedDeserializer()).Build();

consumer.Subscribe(topic);

Console.WriteLine();
Console.WriteLine($"Consuming messages with group id {groupId}..");

Console.CancelKeyPress += delegate
{
    Console.WriteLine();
    Console.WriteLine("I'm being shut down, good bye!");
};

while (true)
{
    var result = consumer.Consume();
    var key = result.Message.Key;
    var value = result.Message.Value;
    var partition = result.Partition;
    var timestamp = result.Message.Timestamp;
    Console.WriteLine($"{key} {value} {partition} {timestamp.UtcDateTime.ToLocalTime():HH:mm:ss}");
    Console.WriteLine(value);
}

class WindowedDeserializer : IDeserializer<Windowed<string>>
{
    public Windowed<string> Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        var serdes = new TimeWindowedSerDes<string>(new StringSerDes(), 10 * 1000);
        var x = serdes.Deserialize(data.ToArray(), context);
        x.Window.ToString();
        return x;
    }
}
