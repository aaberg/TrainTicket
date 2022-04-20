using System.Linq.Expressions;
using MongoDB.Driver;
using TrainTicket.Service.Projections;

namespace TrainTicket.Service.Infrastructure;

public abstract class MongoQueryService
{
    private readonly IMongoClient _mongoClient;
    private readonly MongoDbConfiguration _configuration;
    
    protected abstract string CollectionName { get; }
    
    protected MongoQueryService(MongoDbConfiguration mongoDbConfiguration)
    {
        _configuration = mongoDbConfiguration;
        _mongoClient = new MongoClient(_configuration.ConnectionString);
    }
    
    public IEnumerable<StationDetails> Query(Expression<Func<StationDetails, bool>> predicate)
    {
        return GetCollection().Find(predicate).ToEnumerable();
    }
    

    private IMongoCollection<StationDetails> GetCollection() => GetDatabase().GetCollection<StationDetails>(CollectionName);

    private IMongoDatabase GetDatabase() => _mongoClient.GetDatabase(_configuration.DatabaseName);
}