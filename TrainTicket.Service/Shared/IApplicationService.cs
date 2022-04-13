using TrainTicket.Domain;

namespace TrainTicket.Service.Shared;

public interface IApplicationService<T, TId> where T : AggregateRoot<TId>
{
    Task<T> Handle(object command);
}