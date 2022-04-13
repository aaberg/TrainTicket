using TrainTicket.Service.Projections;

namespace TrainTicket.Central;

public class EventStoreHostedService : IHostedService
{
    private readonly ProjectionManager _projectionManager;
    private readonly ILogger<EventStoreHostedService> _logger;
    
    public EventStoreHostedService(ProjectionManager projectionManager, ILogger<EventStoreHostedService> logger)
    {
        _projectionManager = projectionManager;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting projections manager");
        return _projectionManager.Start();
    
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping and disposing projections manager");
        return _projectionManager.Stop();
    }
}