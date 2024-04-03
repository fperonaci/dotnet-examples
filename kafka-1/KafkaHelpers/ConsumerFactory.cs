using Confluent.Kafka;

namespace KafkaHelpers;

public class ConsumerFactory<TKey, TValue>
{
    private string _bootstrapServers;
    private string _groupId;
    private AutoOffsetReset _offsetReset;

    public ConsumerFactory(
        string bootstrapServers,
        string groupId,
        AutoOffsetReset offsetReset)
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