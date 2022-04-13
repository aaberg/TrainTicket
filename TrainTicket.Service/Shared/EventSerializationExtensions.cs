using System.Text.Json;
using EventStore.Client;
using TrainTicket.Service.Infrastructure;

namespace TrainTicket.Service.Shared;

public static class EventSerializationExtensions
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public static object Deserialize(this ResolvedEvent resolvedEvent)
    {
        var rawMetadata = resolvedEvent.Event.Metadata.ToArray();
        var meta = JsonSerializer.Deserialize<EventMetadata>(rawMetadata, JsonSerializerOptions);
        if (meta == null) throw new Exception("Event metadata is null");
        
        var dataType = Type.GetType(meta.ClrType);
        if (dataType == null) throw new Exception("Unknown event data type");
        
        var @event = JsonSerializer.Deserialize(resolvedEvent.Event.Data.ToArray(), dataType, JsonSerializerOptions);
        if (@event == null) throw new Exception("Error deserializing event. Event data is null.");

        return @event;
    }
    
    public static byte[] Serialize(this object data)
    {
        var outStream = new MemoryStream();
        JsonSerializer.Serialize(outStream, data, JsonSerializerOptions);
        return outStream.ToArray();
    }
}