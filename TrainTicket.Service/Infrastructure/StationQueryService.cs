namespace TrainTicket.Service.Infrastructure;

public class StationQueryService : MongoQueryService
{
    public StationQueryService(MongoDbConfiguration mongoDbConfiguration) : base(mongoDbConfiguration)
    {
    }

    protected override string CollectionName => "StationDetails";
}