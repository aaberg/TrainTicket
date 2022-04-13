using EventStore.Client;

namespace TrainTicket.Service.Shared;

public interface ICheckpointStore
{
    Task<Position> GetCheckpoint();
    Task StoreCheckpoint(Position checkpoint);
}