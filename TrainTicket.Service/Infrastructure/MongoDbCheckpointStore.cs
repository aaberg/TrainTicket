using EventStore.Client;
using MongoDB.Bson;
using MongoDB.Driver;
using TrainTicket.Service.Shared;

namespace TrainTicket.Service.Infrastructure;

public class MongoDbCheckpointStore : ICheckpointStore
{
    private const string CheckpointId = "esCheckpoint"; 
    private readonly MongoClient _mongoClient;
    private readonly MongoDbConfiguration _configuration;

    public MongoDbCheckpointStore(MongoDbConfiguration configuration)
    {
        _configuration = configuration;
        _mongoClient = new MongoClient(configuration.ConnectionString);
    }

    public async Task<Position> GetCheckpoint()
    {
        var result = await GetCollection().FindAsync(
            filter: c => c.Id == CheckpointId
        );

        var checkpoint = result.FirstOrDefault();

        return checkpoint != null ? new Position(checkpoint.CommittedPosition, checkpoint.PreparedPosition) : Position.Start;
    }

    public async Task StoreCheckpoint(Position checkpointPosition)
    {
        var checkpoint = new Checkpoint(CheckpointId, checkpointPosition.CommitPosition, checkpointPosition.PreparePosition);
        
        await GetCollection().ReplaceOneAsync(
            filter: c => c.Id == CheckpointId,
            replacement: checkpoint,
            options: new ReplaceOptions { IsUpsert = true });
    }

    private IMongoCollection<Checkpoint> GetCollection() => GetDatabase().GetCollection<Checkpoint>("EsCheckpoint");

    private IMongoDatabase GetDatabase() => _mongoClient.GetDatabase(_configuration.DatabaseName);
}