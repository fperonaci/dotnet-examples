﻿using KafkaHelpers;
using Streamiz.Kafka.Net;
using Streamiz.Kafka.Net.SerDes;

var bootstrapServers = "localhost:9094";

var topicsManager = new TopicsManager(bootstrapServers);
var topics = topicsManager.GetTopicsNames();

Console.WriteLine("Hello world!");

Console.WriteLine($"Existing topics: {string.Join(", ", topics)}");

Console.WriteLine("Enter the input and output topics");

var inputTopic = Console.ReadLine();
var outputTopic = Console.ReadLine();

var builder = new StreamBuilder();
var keySerdes = new StringSerDes();
var valueSerdes = new StringSerDes();

var kstream = builder.Stream(inputTopic, keySerdes, valueSerdes);

kstream.Filter((key, value) => value.Contains("ciao")).To(outputTopic);

var topology = builder.Build();

var config = new StreamConfig<StringSerDes, StringSerDes>()
{
    ApplicationId = "test_app_id",
    BootstrapServers = bootstrapServers,
    AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest,
    AllowAutoCreateTopics = true,
    Guarantee = ProcessingGuarantee.EXACTLY_ONCE
};

var stream = new KafkaStream(topology, config);

await stream.StartAsync(CancellationToken.None);