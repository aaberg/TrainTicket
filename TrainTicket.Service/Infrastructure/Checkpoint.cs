using EventStore.Client;
using MongoDB.Bson;

namespace TrainTicket.Service.Infrastructure;

public record Checkpoint(String Id, ulong CommittedPosition, ulong PreparedPosition);