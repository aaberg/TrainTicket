namespace TrainTicket.Domain;

public interface IAggregateRoot<out TId>
{
    public TId Id { get; }
    public long Version { get; }

    public void LoadFromHistory(IEnumerable<object> events);
    public IEnumerable<object> GetUncommittedEvents();
    public void ClearUncommittedEvents();
}

public abstract class AggregateRoot<TId> : IAggregateRoot<TId>
{
    public TId Id { get; protected set; }
    public long Version { get; private set; } = -1;
    
    public void LoadFromHistory(IEnumerable<object> events)
    {
        foreach (var @event in events)
        {
            When(@event);
            Version++;
        }
    }
    
    private readonly List<object> _uncommittedEvents = new();

    public IEnumerable<object> GetUncommittedEvents()
    {
        return _uncommittedEvents;
    }

    public void ClearUncommittedEvents()
    {
        _uncommittedEvents.Clear();
    }

    protected AggregateRoot(TId id)
    {
        Id = id;
    }
    protected AggregateRoot() => Id = default!;

    protected abstract void When(object @event);
    protected abstract void EnsureValidState();
    

    protected void Apply(object @event)
    {
        When(@event);
        EnsureValidState();
        _uncommittedEvents.Add(@event);
    }

    
}