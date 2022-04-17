using System.Text;
using System.Text.Json;
using TrainTicket.Domain.Station;
using TrainTicket.Service.Shared;

namespace TrainTicket.Service.Projections;

public class ConsoleProjection: IProjection
{
    public Task Process(Events.IEntityEvent @event)
    {
        Console.WriteLine($"eventype: {@event.GetType().Name}, data: {Encoding.UTF8.GetString(@event.Serialize())}");
        return Task.CompletedTask;
    }
}