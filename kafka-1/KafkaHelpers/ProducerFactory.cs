using Confluent.Kafka;

namespace KafkaHelpers;

public class ProducerFactory<TKey, TValue>
{
    private readonly string _bootstrapServers;

    public ProducerFactory(string bootstrapServers)
    {
        _bootstrapServers = bootstrapServers;
    }

    public IProducer<TKey, TValue> Create()
    {
        var config = new ProducerConfig()
        {
            BootstrapServers = _bootstrapServers,
        };

        var builder = new ProducerBuilder<TKey, TValue>(config);

        return builder.Build();
    }
}
