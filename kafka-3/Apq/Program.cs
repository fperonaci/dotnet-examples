
using Confluent.Kafka;

using Streamiz.Kafka.Net;
using Streamiz.Kafka.Net.SerDes;
using Streamiz.Kafka.Net.Stream;
using Streamiz.Kafka.Net.State;

var topic1 = args[0];
var topic2 = args[1];
var topic3 = args[2];
var topic4 = args[3];

var appId = args.Length > 4 ? args[4] : Guid.NewGuid().ToString();

var builder = new StreamBuilder();

var stream1 = builder.Stream<string, string>(topic1);
var stream2 = builder.Stream<string, string>(topic2);

var table1 = stream2.ToTable();

stream1.Join(
    table1,
    (v1, v2) => $"From inner table join {v1 + v2}")
    .To(topic3);

stream1.Join(
    stream2,
    (v1, v2) => $"From inner stream join {v1 + v2}",
    JoinWindowOptions.Of(TimeSpan.FromSeconds(30)))
    .To(topic4);

// stream1.Join(
//     stream2,
//     (v1, v2) => v1 + v2,
//     JoinWindowOptions.Of(TimeSpan.FromMinutes(1)))
//     .To(topic3);

// stream1.OuterJoin(
//     stream2,
//     (v1, v2) => (v1 + v2).ToUpper(),
//     JoinWindowOptions.Of(TimeSpan.FromMinutes(1)))
//     .To(topic3);

// stream1.LeftJoin(
//     stream2,
//     (v1, v2) => (v1 + v2).ToLower(),
//     JoinWindowOptions.Of(TimeSpan.FromMinutes(1)))
//     .To(topic3);


// var stream2 = stream1.GroupByKey();

// var stream3 = stream2.WindowedBy(TumblingWindowOptions.Of(TimeSpan.FromSeconds(10)));

// var stream4 = stream3.Count().ToStream().Peek((k, v) => Console.WriteLine(k + " " + v.ToString()));

// var serdes = new TimeWindowedSerDes<string>(new StringSerDes(), 10 * 1000);

// stream4.MapValues((k, v) => v.ToString()).To(topic3, keySerdes: serdes, valueSerdes: new StringSerDes());
// stream4.To("topic4", keySerdes: serdes, valueSerdes: new Int64SerDes());



var config = new StreamConfig()
{
    ApplicationId = appId,
    BootstrapServers = "localhost:9094",
    AutoOffsetReset = AutoOffsetReset.Earliest,
    AllowAutoCreateTopics = true,
    Guarantee = ProcessingGuarantee.EXACTLY_ONCE,
    DefaultKeySerDes = new StringSerDes(),
    DefaultValueSerDes = new StringSerDes()
};

var stream = new KafkaStream(builder.Build(), config);

await stream.StartAsync();
