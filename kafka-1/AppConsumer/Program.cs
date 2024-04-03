using Confluent.Kafka;
using KafkaHelpers;

var bootstrapServers = "localhost:9094";

var topicsManager = new TopicsManager(bootstrapServers);
var topics = topicsManager.GetTopicsNames();
var groupIds = topicsManager.GetConsumerGroupsIds();

Console.WriteLine("Hello world!");

Console.WriteLine($"Existing topics: {string.Join(", ", topics)}");

Console.WriteLine($"Existing group ids: {string.Join(", ", groupIds)}");

Console.WriteLine("Enter the input topic and group id");

var topic = Console.ReadLine() ?? throw new NullReferenceException("Cannot read topic name");
var groupId = Console.ReadLine() ?? throw new NullReferenceException("Cannot read group id");
var offsetReset = AutoOffsetReset.Earliest;

var consumerFactory = new ConsumerFactory<string, string>(bootstrapServers, groupId, offsetReset);

using (var consumer = consumerFactory.Create())
{
    consumer.Subscribe(topic);

    Console.WriteLine("Listening for messages..");

    while (true)
    {
        var result = consumer.Consume(CancellationToken.None);
        Console.WriteLine(result.Message.Key);
        Console.WriteLine(result.Message.Value);
    }
}
