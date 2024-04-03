using Confluent.Kafka;
using KafkaHelpers;

var server = "localhost:9094";
var topic = "test-topic";
var group = "test-grous";

var action = args.Length > 0 ? args[0] : null;

if (action is null)
{
    Console.WriteLine("Specify produce or consume");
    return;
}

if (action == "produce")
{
    using var producer = new ProducerFactory<string, string>(server).Create();
    var message = new Message<string, string>()
    {
        Key = "test_key",
        Value = "test_value"
    };
    producer.Produce(topic, message);

    producer.Flush();
}

if (action == "consume")
{
    using var consumer = new ConsumerFactory<string, string>(
        server, group, AutoOffsetReset.Earliest).Create();
    consumer.Subscribe(topic);

    while (true)
    {
        var result = consumer.Consume(CancellationToken.None);
        Console.WriteLine(result.Message.Key);
        Console.WriteLine(result.Message.Value);
    }
}
