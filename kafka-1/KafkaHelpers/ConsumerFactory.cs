using Confluent.Kafka;

namespace KafkaHelpers;

public class ConsumerFactory<TKey, TValue>
{
    private readonly string _bootstrapServers;
    private readonly string _groupId;
    private readonly AutoOffsetReset _offsetReset;

    public ConsumerFactory(string bootstrapServers, string groupId, AutoOffsetReset offsetReset)
    {
        _bootstrapServers = bootstrapServers;
        _groupId = groupId;
        _offsetReset = offsetReset;
    }

    public IConsumer<TKey, TValue> Create()
    {
        var config = new ConsumerConfig()
        {
            BootstrapServers = _bootstrapServers,
            GroupId = _groupId,
            AutoOffsetReset = _offsetReset
        };

        var builder = new ConsumerBuilder<TKey, TValue>(config);

        return builder.Build();
    }
}
