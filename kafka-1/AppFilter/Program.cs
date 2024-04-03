using KafkaHelpers;
using Streamiz.Kafka.Net;
using Streamiz.Kafka.Net.SerDes;

var server = "localhost:9094";

var topicsManager = new TopicsManager(server);
var topics = topicsManager.GetTopicsNames();

Console.WriteLine("Hello world!");

Console.WriteLine($"Existing topics: {string.Join(", ", topics)}");

Console.WriteLine("Enter the input and output topics");

var inputTopic = Console.ReadLine();
var outputTopic = Console.ReadLine();

var builder = new StreamBuilder();
var keySerdes = new StringSerDes();
var valueSerdes = new StringSerDes();

var kstream = builder.Stream(inputTopic, keySerdes, valueSerdes);

kstream.Filter((key, value) => value.Contains("ciao")).To(outputTopic);

var topology = builder.Build();

var config = new StreamConfig()
{
    AllowAutoCreateTopics = true,
    ApplicationId = "test_app_id",
    AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest,
    BootstrapServers = server,
    DefaultKeySerDes = keySerdes,
    DefaultValueSerDes = valueSerdes,
    Guarantee = ProcessingGuarantee.EXACTLY_ONCE,
};

var stream = new KafkaStream(topology, config);

await stream.StartAsync(CancellationToken.None);
