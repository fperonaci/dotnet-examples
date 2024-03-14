using Confluent.Kafka;

var servers = "localhost:9094";
var topic = "test-topic-2";
var group = "test-group-2";

var pconfig = new ProducerConfig()
{
    BootstrapServers = servers
};
var pbuilder = new ProducerBuilder<string, string>(pconfig);

using (var producer = pbuilder.Build())
{
    var message = new Message<string, string>()
    {
        Key = "5",
        Value = "93"
    };
    Console.WriteLine("Producing a message..");
    producer.Produce(topic, message);
    producer.Flush();
}

var cconfig = new ConsumerConfig()
{
    BootstrapServers = servers,
    GroupId = group,
    AutoOffsetReset = AutoOffsetReset.Earliest
};
var cbuilder = new ConsumerBuilder<string, string>(cconfig);

using (var consumer = cbuilder.Build())
{
    consumer.Subscribe(topic);
    Console.WriteLine("Consuming a message..");
    var result = consumer.Consume(CancellationToken.None);
    Console.WriteLine(result.Message.Key);
    Console.WriteLine(result.Message.Value);
}
