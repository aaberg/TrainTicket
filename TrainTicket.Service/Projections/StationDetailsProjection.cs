using MongoDB.Driver;
using TrainTicket.Domain.Station;
using TrainTicket.Service.Infrastructure;

namespace TrainTicket.Service.Projections;

public class StationDetailsProjection : IProjection
{
    private readonly IMongoClient _mongoClient;
    private readonly MongoDbConfiguration _mongoDbConfiguration;
    
    private const string CollectionName = "StationDetails";
    
    public StationDetailsProjection(MongoDbConfiguration mongoDbConfiguration)
    {
        _mongoClient = new MongoClient(mongoDbConfiguration.ConnectionString);
        _mongoDbConfiguration = mongoDbConfiguration;
    }


    public Task Process(Events.IEntityEvent @event) =>
        @event switch
        {
            Events.StationAdded(var id, var name) =>
                GetCollection().InsertOneAsync(new StationDetails
                {
                    Id = id.ToString(),
                    Name = name
                }),
            Events.StationNameChanged(var id, var name) =>
                GetCollection().UpdateOneAsync(
                    filter: Builders<StationDetails>.Filter.Eq(details => details.Id, id.ToString()),
                    update: Builders<StationDetails>.Update.Set(details => details.Name, name)),
            _ => Task.CompletedTask
        };
    
    private IMongoCollection<StationDetails> GetCollection() => 
        _mongoClient.GetDatabase(_mongoDbConfiguration.DatabaseName)
            .GetCollection<StationDetails>(CollectionName);
}