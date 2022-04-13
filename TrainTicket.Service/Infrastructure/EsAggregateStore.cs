using System.Text.Json;
using EventStore.Client;
using TrainTicket.Domain;
using TrainTicket.Service.Shared;

namespace TrainTicket.Service.Infrastructure;

public class EsAggregateStore : IAggregateStore
{
    private readonly EventStoreClient _eventStoreClient;

    public EsAggregateStore(EventStoreClient eventStoreClient)
    {
        _eventStoreClient = eventStoreClient;
    }

    public async Task<T> Load<T, TId>(TId id) where T : IAggregateRoot<TId>
    {
        var streamName = GetStreamName<T, TId>(id);

        object[] events;

        try
        {
            var streamResult = _eventStoreClient.ReadStreamAsync(Direction.Forwards, streamName, StreamPosition.Start);
            events = await streamResult.Select(resolvedEvent => resolvedEvent.Deserialize()).ToArrayAsync();
        }
        catch (StreamNotFoundException e)
        {
            throw new Exceptions.EntityDoesNotExistException("Entity does not exist", e);
        }
        
        var aggregate = (T)(Activator.CreateInstance(typeof(T), true) ??
                            throw new Exception("Could not create instance of aggregate"));
        aggregate.LoadFromHistory(events);
        return aggregate;
    }

    public async Task Save<T, TId>(T aggregate) where T : IAggregateRoot<TId>
    {
        var streamName = GetStreamName<T, TId>(aggregate.Id);

        var uncommittedEvents = aggregate.GetUncommittedEvents().Select(@event => 
            new EventData(Uuid.NewUuid(), @event.GetType().ToString(), @event.Serialize(),
            new EventMetadata(@event.GetType().AssemblyQualifiedName!).Serialize())
        );

        await _eventStoreClient.AppendToStreamAsync(streamName,
            aggregate.Version >= 0 ? StreamRevision.FromInt64(aggregate.Version) : StreamRevision.None,
            uncommittedEvents);
        
        aggregate.ClearUncommittedEvents();
    }
    
    private static string GetStreamName<T, TId>(TId aggregateId) where T : IAggregateRoot<TId>
    {
        return $"{typeof(T).Name}-{aggregateId}";
    }
}