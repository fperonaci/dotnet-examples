
using Confluent.Kafka;

public static partial class Helper<TKey, TValue>
{
    public static void Produce(string server, string topic, int numPartitions)
    {
        var config = new ProducerConfig()
        {
            BootstrapServers = server,
        };

        var producer = new ProducerBuilder<TKey?, TValue?>(config).Build();

        Console.WriteLine($"Producing messages..");

        Console.CancelKeyPress += delegate
        {
            Console.WriteLine("I'm being shut down, good bye!");
        };

        while (true)
        {
            Console.WriteLine("Enter key and value..");
            var input = Console.ReadLine()?.Split(' ');
            var message = new Message<TKey?, TValue?>
            {
                Key = (TKey?)Convert.ChangeType(input?[0], typeof(TKey?)),
                Value = (TValue?)Convert.ChangeType(input?[1], typeof(TValue?))
            };
            producer.Produce(topic, message);
            producer.Flush();
        }
    }
}

