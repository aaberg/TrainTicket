namespace TrainTicket.Domain.Station;

public static class Events
{
    public interface IEntityEvent
    {
        Guid Id{get;}
    }
    
    public record StationAdded(Guid Id, string Name) : IEntityEvent;
    public record StationNameChanged(Guid Id, string Name) : IEntityEvent;
    
}