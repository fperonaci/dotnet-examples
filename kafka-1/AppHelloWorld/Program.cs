using Confluent.Kafka;
using KafkaHelpers;

var bootstrapServers = "localhost:9094";
var topicName = "test_topic";
var numPartitions = 1;

var groupId = "test_group_id";
var offsetReset = AutoOffsetReset.Earliest;

var topicsManager = new TopicsManager(bootstrapServers);

Console.WriteLine("Hello world!");

await topicsManager.CreateTopicIfNotExists(topicName, numPartitions);

var producerFactory = new ProducerFactory<string, string>(bootstrapServers);

using (var producer = producerFactory.Create())
{
    var message = new Message<string, string>()
    {
        Key = "test_key",
        Value = "test_value"
    };
    producer.Produce(topicName, message);

    producer.Flush();
}

var consumerFactory = new ConsumerFactory<string, string>(bootstrapServers, groupId, offsetReset);

using (var consumer = consumerFactory.Create())
{
    consumer.Subscribe(topicName);

    while (true)
    {
        var result = consumer.Consume(CancellationToken.None);
        Console.WriteLine(result.Message.Key);
        Console.WriteLine(result.Message.Value);
    }
}
