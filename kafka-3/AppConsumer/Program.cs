using Confluent.Kafka;
using Helpers;

var server = args[0];
var topic = args[1];
var ktype = args[2];
var vtype = args[3];

var manager = new TopicsManager(server);

if (!manager.GetTopicsNames().Contains(topic))
{
    Console.WriteLine("Topic does not exist, shutting down");
    return;
}

var config = new ConsumerConfig()
{
    BootstrapServers = server,
    GroupId = Guid.NewGuid().ToString(),
    AutoOffsetReset = AutoOffsetReset.Earliest
};

var builder = new ConsumerBuilder<int, int>(config);

var consumer = builder.Build();

consumer.Subscribe(topic);

Console.WriteLine("Subscribed to topic");

Console.WriteLine("Consuming messages");

while(true)
{
    var result = consumer.Consume();
    Console.WriteLine($"{result.Message.Key}, {result.Message.Value}");
}
