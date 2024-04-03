using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace KafkaHelpers;

public class TopicsManager
{
    private readonly AdminClientBuilder _builder;

    public TopicsManager(string bootstrapServers)
    {
        var config = new AdminClientConfig()
        {
            BootstrapServers = bootstrapServers
        };

        _builder = new AdminClientBuilder(config);
    }

    public IEnumerable<string> GetConsumerGroupsIds()
    {
        using var client = _builder.Build();

        return client.ListConsumerGroupsAsync().Result.Valid.Select(x => x.GroupId);
    }

    public IEnumerable<string> GetTopicsNames()
    {
        using var client = _builder.Build();

        return client.GetMetadata(TimeSpan.FromSeconds(60)).Topics.Select(x => x.Topic);
    }

    public void CreateTopicIfNotExists(string name, int numPartitions)
    {
        if (GetTopicsNames().Contains(name))
        {
            return;
        }

        using var client = _builder.Build();

        client.CreateTopicsAsync([new() { Name = name, NumPartitions = numPartitions }]).Wait();
    }

    public void DeleteTopicIfExists(string name)
    {
        if (!GetTopicsNames().Contains(name))
        {
            return;
        }

        using var client = _builder.Build();

        client.DeleteTopicsAsync([name]).Wait();
    }
}
