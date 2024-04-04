
using Confluent.Kafka;

using Streamiz.Kafka.Net;
using Streamiz.Kafka.Net.SerDes;
using Streamiz.Kafka.Net.Stream;


var builder = new StreamBuilder();

var kstream1 = builder.Stream<string, string>(args[0]);
var kstream2 = builder.Stream<string, string>(args[1]);

kstream1.Join(
    kstream2,
    (v1, v2) => v1 + v2,
    JoinWindowOptions.Of(TimeSpan.FromMinutes(1)))
    .To(args[2]);

var config = new StreamConfig()
{
    ApplicationId = "eta-calculation-poq",
    BootstrapServers = "localhost:9094",
    AutoOffsetReset = AutoOffsetReset.Earliest,
    AllowAutoCreateTopics = true,
    Guarantee = ProcessingGuarantee.EXACTLY_ONCE,
    DefaultKeySerDes = new StringSerDes(),
    DefaultValueSerDes = new Int32SerDes()
};

var stream = new KafkaStream(builder.Build(), config);

await stream.StartAsync();
