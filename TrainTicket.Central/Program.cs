using Confluent.Kafka;
using EventStore.Client;
using Microsoft.OpenApi.Models;
using TrainTicket.Central;
using TrainTicket.Service.Infrastructure;
using TrainTicket.Service.Projections;
using TrainTicket.Service.Shared;
using TrainTicket.Service.Station;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Train station management API", Version = "v1" }));

var eventStoreConfiguration = new EventStoreConfiguration();
builder.Configuration.Bind("EventStoreConfiguration", eventStoreConfiguration);
var mongoConfiguration = new MongoDbConfiguration();
builder.Configuration.Bind("MongoDbConfiguration", mongoConfiguration);
var kafkaConfiguration = new KafkaConfiguration();
builder.Configuration.Bind("KafkaConfiguration", kafkaConfiguration);

builder.Services.AddSingleton(eventStoreConfiguration);
builder.Services.AddSingleton(mongoConfiguration);
builder.Services.AddSingleton(kafkaConfiguration);

builder.Services.AddSingleton<IAggregateStore, EsAggregateStore>();
builder.Services.AddScoped<StationService>();
builder.Services.AddSingleton<ICheckpointStore, MongoDbCheckpointStore>();

// Add eventstore client
builder.Services.AddSingleton(provider =>
{
    var configuration = provider.GetService<EventStoreConfiguration>();
    var settings = EventStoreClientSettings.Create(configuration!.ConnectionString);
    return new EventStoreClient(settings);
});

// Add Projections 
builder.Services.AddSingleton(provider =>
{
    var eventStoreClient = provider.GetService<EventStoreClient>();
    var checkpointStore = provider.GetService<ICheckpointStore>();
    var logger = provider.GetService<ILogger<ProjectionManager>>();
    
    var kafkaConfig = provider.GetService<KafkaConfiguration>()!;
    var producerConfig = new ProducerConfig(new ClientConfig { BootstrapServers = kafkaConfig.Server });
    
    return new ProjectionManager(eventStoreClient!, checkpointStore!, new IProjection[]
    {
        new ConsoleProjection(),
        new KafkaProjection(producerConfig)
    }, logger!);
});

builder.Services.AddSingleton<IHostedService, EventStoreHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Train station management API v1"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();