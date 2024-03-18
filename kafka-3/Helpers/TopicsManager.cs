using Confluent.Kafka;

namespace Helpers;

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

    public IEnumerable<string> GetTopicsNames()
    {
        using var client = _clientBuilder.Build();

        return client.GetMetadata(TimeSpan.FromSeconds(60)).Topics.Select(x => x.Topic);
    }
}
