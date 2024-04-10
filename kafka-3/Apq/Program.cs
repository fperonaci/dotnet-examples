
using Confluent.Kafka;

using Streamiz.Kafka.Net;
using Streamiz.Kafka.Net.SerDes;
using Streamiz.Kafka.Net.Stream;

var topic1 = args[0];
var topic2 = args[1];
var topic3 = args[2];
var topic4 = args[3];

var appId = args.Length > 4 ? args[4] : Guid.NewGuid().ToString();

var builder = new StreamBuilder();

var stringSerDes = new StringSerDes();

var stream1 = builder.Stream(topic1, stringSerDes, stringSerDes);

// var stream2 = builder.Stream(topic2, stringSerDes, stringSerDes);

// stream1.Map((k,v) => KeyValuePair.Create(k.ToUpper(), v)).To(topic2);

// var table1 = stream2.ToTable();

// stream1.Join(
//     table1,
//     (v1, v2) => $"From inner table join {v1 + v2}")
//     .To(topic3);

// stream1.Join(
//     stream2,
//     (v1, v2) => $"From inner stream join {v1 + v2}",
//     JoinWindowOptions.Of(TimeSpan.FromSeconds(30)))
//     .To(topic4);

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

//var stream2 = stream1.GroupBy((k, v) => k[..1] + v[..1]);

// var stream3 = stream2.WindowedBy(TumblingWindowOptions.Of(TimeSpan.FromSeconds(10)));

// var stream4 = stream2.Count().ToStream(); //.Peek((k, v) => Console.WriteLine(k + " " + v.ToString()));

// var serdes = new TimeWindowedSerDes<string>(new StringSerDes(), 10 * 1000);

// stream4.MapValues((k, v) => v.ToString()).To(topic2);

//stream2.Reduce((v1, v2) => v1 + v2).ToStream().To(topic2);

//stream2.Aggregate(() => "INIZIO", (k, v, vr) => vr + k + v).ToStream().To(topic3);

var config = new StreamConfig()
{
    ApplicationId = appId,
    BootstrapServers = "localhost:9094",
    AutoOffsetReset = AutoOffsetReset.Earliest,
    AllowAutoCreateTopics = true,
    Guarantee = ProcessingGuarantee.EXACTLY_ONCE,
    DefaultKeySerDes = stringSerDes,
    DefaultValueSerDes = stringSerDes
};

var stream = new KafkaStream(builder.Build(), config);

await stream.StartAsync();
