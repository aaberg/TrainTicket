using TrainTicket.Domain.Station;

namespace TrainTicket.Service.Projections;

public interface IProjection
{
    Task Process(Events.IEntityEvent @event);
}