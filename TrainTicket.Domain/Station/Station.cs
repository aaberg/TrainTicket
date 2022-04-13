namespace TrainTicket.Domain.Station;

public class Station : AggregateRoot<StationId>
{
    public StationName Name { get; private set; } = default!;
    
    public Station(StationId id, StationName name) : base(id)
    {
        Apply(new Events.StationAdded(id, name));
    }

    public void ChangeName(StationName newName)
    {
        Apply(new Events.StationNameChanged(Id, newName));
    }
    

    protected override void When(object @event)
    {
        switch (@event)
        {
            case Events.StationAdded e:
                Name = e.Name;
                Id = e.Id;
                break;
            case Events.StationNameChanged e:
                Name = e.Name;
                break;
        }
    }

    protected override void EnsureValidState()
    {
        if (Id == null)
        {
            throw new DomainExceptions.InvalidEntityState(this, "Id is null");
        }
    }
    
    // Satisfy serialization requirements
    protected Station() { }
}