using System.Text;
using Confluent.Kafka;
using TrainTicket.Domain.Station;
using TrainTicket.Service.Shared;

namespace TrainTicket.Service.Projections;

public class KafkaProjection : IProjection
{
    private readonly ProducerConfig _config;
    private readonly IProducer<string, string> _producer;
    private const string Topic = "station-events";

    public KafkaProjection(ProducerConfig config)
    {
        _config = config;
        _producer = new ProducerBuilder<string, string>(_config).Build();
    }
    
    public Task Process(Events.IEntityEvent @event)
    {
        var message = new Message<string, string>
        {
            Value = Encoding.UTF8.GetString(@event.Serialize()),
            Key = @event.Id.ToString()
        };
        
        return _producer.ProduceAsync(Topic, message);
    }
}