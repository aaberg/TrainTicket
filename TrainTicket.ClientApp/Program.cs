// See https://aka.ms/new-console-template for more information

using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using TrainTicket.ClientApp;

Console.WriteLine("Starting application...");

IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var kafkaConfig = new KafkaConfiguration();
config.Bind("kafka", kafkaConfig);


var kafkaConsumerConfig = new ConsumerConfig
{
    BootstrapServers = kafkaConfig.BootstrapServers,
    GroupId = kafkaConfig.GroupId,
    AutoOffsetReset = AutoOffsetReset.Earliest
};


