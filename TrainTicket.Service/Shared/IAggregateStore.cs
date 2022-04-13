using TrainTicket.Domain;

namespace TrainTicket.Service.Shared;

public interface IAggregateStore
{
    Task<T> Load<T, TId>(TId id) where T : IAggregateRoot<TId>;
    
    Task Save<T, TId>(T aggregate) where T : IAggregateRoot<TId>;
}