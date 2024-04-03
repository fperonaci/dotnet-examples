using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace KafkaHelpers;

public class TopicsManager
{
    private readonly AdminClientBuilder _clientBuilder;

    public TopicsManager(string bootstrapServers)
    {
        var config = new AdminClientConfig()
        {
            BootstrapServers = bootstrapServers
        };

        _clientBuilder = new AdminClientBuilder(config);
    }

    public IEnumerable<string> GetConsumerGroupsIds()
    {
        using var client = _clientBuilder.Build();

        return client.ListConsumerGroupsAsync().Result.Valid.Select(x => x.GroupId);
    }

    public IEnumerable<string> GetTopicsNames()
    {
        using var client = _clientBuilder.Build();

        return client.GetMetadata(TimeSpan.FromSeconds(60)).Topics.Select(x => x.Topic);
    }

    public async Task CreateTopicIfNotExists(string name, int numPartitions)
    {
        if (GetTopicsNames().Contains(name))
        {
            return;
        }

        using var client = _clientBuilder.Build();

        var topicSpecifications = new TopicSpecification[]{
            new TopicSpecification()
            {
                Name = name,
                NumPartitions = numPartitions
            }
        };

        await client.CreateTopicsAsync(topicSpecifications);
    }

    public async Task DeleteTopicIfExists(string name)
    {
        if (!GetTopicsNames().Contains(name))
        {
            return;
        }

        using var client = _clientBuilder.Build();

        await client.DeleteTopicsAsync([name]);
    }
}
