using System.Text.Json;
using TrainTicket.Domain.Station;

namespace TrainTicket.Service.Projections;

public class ConsoleProjection: IProjection
{
    public Task Process(Events.IEntityEvent @event)
    {
        Console.WriteLine($"eventype: {@event.GetType().Name}, data: {JsonSerializer.Serialize(@event)}");
        return Task.CompletedTask;
    }
}