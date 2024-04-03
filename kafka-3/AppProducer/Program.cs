using Confluent.Kafka;

var server = args[0];
var topic = args[1];
var ktype = args[2];
var vtype = args[3];

var config = new ProducerConfig()
{
    BootstrapServers = server,
};

var builder = new ProducerBuilder<int, int>(config);

var producer = builder.Build();

Console.WriteLine("Producing messages");

while(true)
{
    int.TryParse(Console.ReadLine(), out int key);
    int.TryParse(Console.ReadLine(), out int value);
    var message = new Message<int, int>
    {
        Key = key,
        Value = value
    };
    producer.Produce(topic, message);
    producer.Flush();
}
