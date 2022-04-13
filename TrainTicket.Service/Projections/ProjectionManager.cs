using EventStore.Client;
using Microsoft.Extensions.Logging;
using TrainTicket.Domain.Station;
using TrainTicket.Service.Shared;

namespace TrainTicket.Service.Projections;

public class ProjectionManager
{
    private readonly EventStoreClient _eventStoreClient;
    private readonly IProjection[] _projections;
    private readonly ICheckpointStore _checkpointStore;
    private readonly ILogger<ProjectionManager> _logger;

    private StreamSubscription _subscription = default!;
    
    public ProjectionManager(EventStoreClient eventStoreClient, ICheckpointStore checkpointStore, IProjection[] projections, ILogger<ProjectionManager> logger)
    {
        _eventStoreClient = eventStoreClient;
        _projections = projections;
        _checkpointStore = checkpointStore;
        _logger = logger;
    }

    public async Task Start()
    {
        var checkpoint = await _checkpointStore.GetCheckpoint();

        _subscription = await _eventStoreClient.SubscribeToAllAsync(FromAll.After(checkpoint), EventAppeared);
    }

    public Task Stop()
    {
        _subscription.Dispose();
        return Task.CompletedTask;
    }
    
    private async Task EventAppeared(StreamSubscription subscription, ResolvedEvent resolvedEvent, CancellationToken cancellationToken)
    {
        try
        {
            if (resolvedEvent.Event.EventType.StartsWith("$")) return;
            
            var @event = resolvedEvent.Deserialize() as Events.IEntityEvent;
            if (@event == null)
            {
                _logger.LogWarning("Trying to process a null event. Skipping.");
                return;
            }

            foreach (var projection in _projections)
            {
                await projection.Process(@event);
            }

            if (resolvedEvent.OriginalPosition.HasValue)
                await _checkpointStore.StoreCheckpoint(resolvedEvent.OriginalPosition.Value);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error int projection manager processing event, event: {@event}", resolvedEvent);
            throw;
        }
    }
}