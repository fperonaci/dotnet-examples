using Confluent.Kafka;
using KafkaHelpers;

var bootstrapServers = "localhost:9094";

var topicsManager = new TopicsManager(bootstrapServers);
var topics = topicsManager.GetTopicsNames();

Console.WriteLine("Hello world!");

Console.WriteLine($"Topics in Kafka: {string.Join(", ", topics)}");

Console.WriteLine("Enter the output topic (will create if not exists)");

var topic = Console.ReadLine() ?? throw new NullReferenceException("Cannot read topic name");
var numPartitions = 1;

await topicsManager.CreateTopicIfNotExists(topic, numPartitions);

var producerFactory = new ProducerFactory<string, string>(bootstrapServers);

using var producer = producerFactory.Create();

while (true)
{
    Console.WriteLine("Enter key and value of the message");

    var key = Console.ReadLine() ?? throw new NullReferenceException("Cannot read key");
    var value = Console.ReadLine() ?? throw new NullReferenceException("Cannot read value");

    var message = new Message<string, string>()
    {
        Key = key,
        Value = value
    };

    Console.WriteLine("Sending messages..");

    producer.Produce(topic, message);

    Console.WriteLine("Flushing..");

    producer.Flush();
}
