using TrainTicket.Domain.Station;
using TrainTicket.Service.Shared;

namespace TrainTicket.Service.Station;

public class StationService : IApplicationService<Domain.Station.Station, StationId>
{
    private readonly IAggregateStore _aggregateStore;
    
    public StationService(IAggregateStore aggregateStore)
    {
        _aggregateStore = aggregateStore;
    }

    public Task<Domain.Station.Station> Handle(object command) => command switch
    {
        Commands.Add cmd => HandleCreate(cmd),
        Commands.ChangeName cmd => HandleUpdate(cmd.Id, cmd),
        _ => throw new NotSupportedException("Command not supported.")
    };

    private async Task<Domain.Station.Station> HandleCreate(Commands.Add cmd)
    {
        var station = new Domain.Station.Station(cmd.Id, cmd.Name);
        await _aggregateStore.Save<Domain.Station.Station, StationId>(station);

        return station;
    }

    private async Task<Domain.Station.Station> HandleUpdate(StationId stationId, object command)
    {
        var station = await _aggregateStore.Load<Domain.Station.Station, StationId>(stationId);
        
        switch (command)
        {
            case Commands.ChangeName cmd:
                station.ChangeName(cmd.NewName);
                break;
            default:
                throw new NotSupportedException("Command not supported.");
        }
        
        await _aggregateStore.Save<Domain.Station.Station, StationId>(station);

        return station;
    }
}