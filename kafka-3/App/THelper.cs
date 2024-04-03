using Confluent.Kafka;

namespace App;

public static class THelper
{
    public static IEnumerable<string> GetConsumerGroupsIds(string server)
    {
        var config = new AdminClientConfig()
        {
            BootstrapServers = server
        };

        using var client = new AdminClientBuilder(config).Build();

        return client.ListConsumerGroupsAsync().Result.Valid.Select(x => x.GroupId);
    }

    public static IEnumerable<string> GetTopicsNames(string server)
    {
        var config = new AdminClientConfig()
        {
            BootstrapServers = server
        };

        using var client = new AdminClientBuilder(config).Build();

        return client.GetMetadata(TimeSpan.FromSeconds(60)).Topics.Select(x => x.Topic);
    }

    public static IEnumerable<int> GetNumberOfPartitions(string server)
    {
        var config = new AdminClientConfig()
        {
            BootstrapServers = server
        };

        using var client = new AdminClientBuilder(config).Build();

        return client.GetMetadata(TimeSpan.FromSeconds(60)).Topics.Select(x => x.Partitions.Count);
    }

    public static void CreateTopicIfNotExists(string server, string name, int numPartitions)
    {
        if (GetTopicsNames(server).Contains(name))
        {
            return;
        }

        var config = new AdminClientConfig()
        {
            BootstrapServers = server
        };

        using var client = new AdminClientBuilder(config).Build();

        client.CreateTopicsAsync([new() { Name = name, NumPartitions = numPartitions }]).Wait();
    }

    public static void DeleteTopicIfExists(string server, string name)
    {
        if (!GetTopicsNames(server).Contains(name))
        {
            return;
        }

        var config = new AdminClientConfig()
        {
            BootstrapServers = server
        };

        using var client = new AdminClientBuilder(config).Build();

        client.DeleteTopicsAsync([name]).Wait();
    }
}
